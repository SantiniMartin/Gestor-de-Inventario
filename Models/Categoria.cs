using System.ComponentModel.DataAnnotations;

namespace GestionDeInventario.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        // Relación con productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
