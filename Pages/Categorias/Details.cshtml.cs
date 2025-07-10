using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionDeInventario.Data;
using GestionDeInventario.Models;

namespace GestionDeInventario.Pages.Categorias
{
    public class DetailsModel : PageModel
    {
        private readonly GestionDeInventario.Data.AppDbContext _context;

        public DetailsModel(GestionDeInventario.Data.AppDbContext context)
        {
            _context = context;
        }

        public Categoria Categoria { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            else
            {
                Categoria = categoria;
            }
            return Page();
        }
    }
}
