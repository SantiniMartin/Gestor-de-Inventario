using Microsoft.EntityFrameworkCore;

namespace GestionDeInventario.Data
{
    public class GestionDeInventarioContext : DbContext
    {
        public GestionDeInventarioContext (DbContextOptions<GestionDeInventarioContext> options)
            : base(options)
        {
        }

        public DbSet<GestionDeInventario.Models.Categoria> Categoria { get; set; } = default!;
    }
}
