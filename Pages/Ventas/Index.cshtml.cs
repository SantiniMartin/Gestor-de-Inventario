using GestionDeInventario.Data;
using GestionDeInventario.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GestionDeInventario.Pages.Ventas
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Venta> Ventas { get; set; } = new();

        public async Task OnGetAsync()
        {
            Ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }
    }
}
