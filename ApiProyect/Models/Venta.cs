namespace ApiProyect.Models;

public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Completada";

    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public string? UsuarioId { get; set; } // FK opcional a IdentityUser

    public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}