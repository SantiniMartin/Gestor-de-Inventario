namespace ApiProyect.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string? ImagenUrl { get; set; }
    public string? Marca { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    public string? CategoriaNombre { get; set; } 

    public int CategoriaId { get; set; }

    public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
}