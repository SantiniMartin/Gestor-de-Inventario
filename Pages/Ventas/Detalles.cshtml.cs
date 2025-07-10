using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionDeInventario.Data;
namespace GestionDeInventario.Pages.Ventas

{
    public class DetallesModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetallesModel(AppDbContext context)
        {
            _context = context;
        }

        public Models.Venta? Venta { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Venta == null)
                return NotFound();

            return Page();
        }
    }
}
