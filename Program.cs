using GestionDeInventario.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Agregar Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddDbContext<GestionDeInventarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionDeInventarioContext") ?? throw new InvalidOperationException("Connection string 'GestionDeInventarioContext' not found.")));

// Registrar el DbContext con la cadena de conexi�n
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar Identity
builder.Services.AddDefaultIdentity<GestionDeInventario.Models.ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // Requiere login para todas las páginas
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Logout");
    // Bloquea el registro público
    options.Conventions.AuthorizePage("/Account/Register");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // <-- Importante: antes de Authorization
app.UseAuthorization();

app.MapRazorPages();

app.Run();