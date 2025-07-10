using Microsoft.EntityFrameworkCore;
using ApiProyect.Models;

namespace ApiProyect.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Productos)
            .WithOne(p => p.Categoria!)
            .HasForeignKey(p => p.CategoriaId);

        modelBuilder.Entity<Producto>()
            .HasMany(p => p.DetallesVenta)
            .WithOne(dv => dv.Producto!)
            .HasForeignKey(dv => dv.ProductoId);

        modelBuilder.Entity<Venta>()
            .HasMany(v => v.Detalles)
            .WithOne(dv => dv.Venta!)
            .HasForeignKey(dv => dv.VentaId);

        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Ventas)
            .WithOne(v => v.Cliente!)
            .HasForeignKey(v => v.ClienteId);
    }
}