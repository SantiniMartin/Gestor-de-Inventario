namespace ApiProyect.Models
{
    public class ProductoDetailDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Marca { get; set; }
        public string CategoriaNombre { get; set; }
    }
}
