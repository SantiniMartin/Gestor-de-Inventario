namespace GestionDeInventario.Models
{
    public class VentaFormViewModel
    {
        public int ClienteId { get; set; }

        public List<ProductoSeleccionado> ProductosSeleccionados { get; set; } = new();
    }

    public class ProductoSeleccionado
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Cantidad { get; set; }  // cantidad elegida por el usuario
    }
}
