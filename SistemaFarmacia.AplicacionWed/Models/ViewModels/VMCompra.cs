namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMCompra
    {
        public int IdCompra { get; set; }

        public DateTime? FechaHoraCompra { get; set; }

        public decimal? TotalCompra { get; set; } //precio al que se compro el producto

        public int? CantidadProductosCompra { get; set; } //Productos que entran al sistema

        public int? IdProveedor { get; set; }

        public int? LoteInterno { get; set; }

        public int? LoteProveedor { get; set; }

        public DateTime? fechaVencimiento { get; set; }

    }
}
