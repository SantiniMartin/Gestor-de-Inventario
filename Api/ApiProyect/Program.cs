using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ApiProyect.Data;
using ApiProyect.Models;
var builder = WebApplication.CreateBuilder(args);

Env.Load(); // Carga las variables del archivo .env

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSPA",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Origen de la SPA
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowSPA");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/productos", async (AppDbContext db) =>
{
    var productos = await db.Productos.ToListAsync();
    return Results.Ok(productos);
})
.WithName("GetProductos");

app.MapGet("/clientes", async (AppDbContext db) =>
{
    var clientes = await db.Clientes.ToListAsync();
    return Results.Ok(clientes);
})
.WithName("GetClientes");

app.MapGet("/categorias", async (AppDbContext db) =>
{
    var categorias = await db.Categorias.ToListAsync();
    return Results.Ok(categorias);
})
.WithName("GetCategorias");

app.MapPost("/postcategorias", async (AppDbContext db, Categoria categoria) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute("PostCategorias", new { id = categoria.Id }, categoria);
})
.WithName("PostCategorias");

app.MapPost("/postproductos", async (AppDbContext db, Producto producto) =>
{
       // Validar que la categoría exista
    var categoriaExiste = await db.Categorias.AnyAsync(c => c.Id == producto.CategoriaId);

    if (!categoriaExiste)
    {
        return Results.BadRequest($"La categoría con ID {producto.CategoriaId} no existe.");
    }
    var categoria_Nombre = await db.Categorias.FindAsync(producto.CategoriaId);
    producto.CategoriaNombre = categoria_Nombre?.Nombre;


    db.Productos.Add(producto);
    await db.SaveChangesAsync();
    return Results.CreatedAtRoute("PostProductos", new { id = producto.Id }, producto);
})
.WithName("PostProductos");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}