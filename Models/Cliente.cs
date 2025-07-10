using System.ComponentModel.DataAnnotations;

namespace GestionDeInventario.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
