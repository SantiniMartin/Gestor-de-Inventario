using GestionDeInventario.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionDeInventario.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Venta> Ventas => Set<Venta>();
        public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();
    }
}
