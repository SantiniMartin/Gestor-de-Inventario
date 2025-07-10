using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionDeInventario.Data;
using GestionDeInventario.Models;

namespace GestionDeInventario.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly GestionDeInventario.Data.AppDbContext _context;

        public IndexModel(GestionDeInventario.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Producto> Producto { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Producto = await _context.Productos
                .Include(p => p.Categoria).ToListAsync();
        }
    }
}
