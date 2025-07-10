using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace ApiProyect.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
    Env.Load(); // Carga el .env manualmente

    var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("La variable POSTGRES_CONNECTION no está definida o está vacía.");
    }

    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    return new AppDbContext(optionsBuilder.Options);
}

}