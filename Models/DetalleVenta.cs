using System.ComponentModel.DataAnnotations;

namespace GestionDeInventario.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        // FKs
        public int VentaId { get; set; }
        public Venta? Venta { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
