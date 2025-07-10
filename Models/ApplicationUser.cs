using Microsoft.AspNetCore.Identity;
using System;

namespace GestionDeInventario.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Puedes agregar propiedades adicionales aquí si lo necesitas
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
    }
} 