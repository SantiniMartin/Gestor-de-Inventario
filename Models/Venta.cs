using System.ComponentModel.DataAnnotations;

namespace GestionDeInventario.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        // FK
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
