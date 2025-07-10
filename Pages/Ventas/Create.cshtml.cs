using GestionDeInventario.Data;
using GestionDeInventario.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GestionDeInventario.Pages.Ventas
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VentaFormViewModel VentaForm { get; set; } = new();

        public List<Cliente> Clientes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Clientes = await _context.Clientes.ToListAsync();

            var productos = await _context.Productos.ToListAsync();

            VentaForm.ProductosSeleccionados = productos.Select(p => new ProductoSeleccionado
            {
                ProductoId = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var cliente = await _context.Clientes.FindAsync(VentaForm.ClienteId);
            if (cliente == null)
            {
                ModelState.AddModelError("", "Cliente no válido.");
                return Page();
            }

            var detalles = new List<DetalleVenta>();
            decimal total = 0;

            foreach (var p in VentaForm.ProductosSeleccionados.Where(p => p.Cantidad > 0))
            {
                var productoDb = await _context.Productos.FindAsync(p.ProductoId);
                if (productoDb == null || productoDb.Stock < p.Cantidad)
                {
                    ModelState.AddModelError("", $"Stock insuficiente para el producto {p.Nombre}.");
                    return Page();
                }

                var subtotal = p.Cantidad * productoDb.Precio;

                detalles.Add(new DetalleVenta
                {
                    ProductoId = p.ProductoId,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = productoDb.Precio,
                });

                productoDb.Stock -= p.Cantidad;
                total += subtotal;
            }

            if (!detalles.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto.");
                return Page();
            }

            var venta = new Venta
            {
                ClienteId = VentaForm.ClienteId,
                Fecha = DateTime.Now,
                Total = total,
                Detalles = detalles
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
