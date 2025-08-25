namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMPrecioProducto
    {
        public int IdPrecioProducto { get; set; }

        public decimal? Precio { get; set; }

        public int? IdProducto { get; set; }

        public int? IdProveedor { get; set; }
    }
}
