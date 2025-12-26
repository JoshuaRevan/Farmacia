namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMProducto
    {
        public int IdProducto { get; set; }

        public string? NombreProducto { get; set; }

        public string? Descripcion { get; set; }

        public string? Ubicacion { get; set; }

        public int? IdMarca { get; set; }
        public string? NombreMarca { get; set; }

        public int? IdPresentacion { get; set; }
        public string? NombrePresentacion { get; set; }

        
    }
}
