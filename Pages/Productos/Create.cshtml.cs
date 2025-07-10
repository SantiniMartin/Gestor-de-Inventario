using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionDeInventario.Models;

namespace GestionDeInventario.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly GestionDeInventario.Data.AppDbContext _context;

        public CreateModel(GestionDeInventario.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return Page();
        }

        [BindProperty]
        public Producto Producto { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Productos.Add(Producto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
