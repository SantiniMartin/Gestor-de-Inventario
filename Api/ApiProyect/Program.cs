using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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

app.MapGet("/productos/{id}", async (int id, AppDbContext db) =>
{
    var producto = await db.Productos
        .Where(p => p.Id == id)
        .FirstOrDefaultAsync();

    if (producto == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(producto);
})
.WithName("GetProductoById");

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
    return Results.CreatedAtRoute("GetProductoById", new { id = producto.Id }, producto);
})
.WithName("PostProductos");

app.MapPut("/productos/{id}", async (int id, Producto productoUpdate, AppDbContext db) =>
{
    var producto = await db.Productos.FindAsync(id);
    if (producto is null) return Results.NotFound();

    // Aquí asumimos que el frontend envía el nuevo stock en el objeto
    // podrías también enviar solo la cantidad comprada y restarla aquí.
    producto.Stock = productoUpdate.Stock;
    
    await db.SaveChangesAsync();
    return Results.Ok(producto);
})
.WithName("UpdateProducto");

app.MapPost("/register", async (UsuarioDto usuarioDto, AppDbContext db) =>
{
    var usuarioExistente = await db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario);
    if (usuarioExistente != null)
    {
        return Results.BadRequest("El nombre de usuario ya existe.");
    }

    var usuario = new Usuario
    {
        NombreUsuario = usuarioDto.NombreUsuario,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password)
    };

    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();

    return Results.StatusCode(201); // Created
});

app.MapPost("/login", async (UsuarioDto usuarioDto, AppDbContext db) =>
{
    var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario);

    if (usuario == null || !BCrypt.Net.BCrypt.Verify(usuarioDto.Password, usuario.PasswordHash))
    {
        return Results.Unauthorized();
    }

    return Results.Ok("Login exitoso.");
});

app.MapPost("/forgot-password", async (ForgotPasswordDto forgotPasswordDto, AppDbContext db) =>
{
    var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == forgotPasswordDto.NombreUsuario);
    if (usuario == null)
    {
        // Log de depuración para confirmar que el usuario no fue encontrado.
        Console.WriteLine($"[DEBUG] Intento de reseteo para un usuario no encontrado: '{forgotPasswordDto.NombreUsuario}'");
        // Se devuelve Ok para no revelar si un usuario existe o no (previene enumeración de usuarios)
        return Results.Ok("Si existe una cuenta con ese nombre de usuario, se ha enviado un enlace para restablecer la contraseña.");
    }

    // Generar un token seguro
    var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    usuario.PasswordResetToken = token;
    usuario.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15); // El token es válido por 15 minutos

    await db.SaveChangesAsync();

    // --- EN PRODUCCIÓN: ESTA SECCIÓN DEBE ENVIAR UN EMAIL ---
    Console.WriteLine($"Password Reset Token for {usuario.NombreUsuario}: {token}");

    // Devolver el token directamente en la respuesta para que el frontend construya la ruta.
    return Results.Ok(new { token });
})
.WithName("ForgotPassword");

app.MapPost("/reset-password", async (ResetPasswordDto resetPasswordDto, AppDbContext db) =>
{
    var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.PasswordResetToken == resetPasswordDto.Token);

    if (usuario == null || usuario.ResetTokenExpires < DateTime.UtcNow)
    {
        return Results.BadRequest("El token es inválido o ha expirado.");
    }

    // Actualizar contraseña
    usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);
    usuario.PasswordResetToken = null; // Limpiar el token después de usarlo
    usuario.ResetTokenExpires = null;

    await db.SaveChangesAsync();

    return Results.Ok("La contraseña ha sido restablecida con éxito.");
})
.WithName("ResetPassword");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}