using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionDeInventario.Models;

namespace GestionDeInventario.Pages.Categorias
{
    public class IndexModel : PageModel
    {
        private readonly GestionDeInventario.Data.AppDbContext _context;

        public IndexModel(GestionDeInventario.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Categoria> Categoria { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Categoria = await _context.Categorias.ToListAsync();
        }
    }
}
