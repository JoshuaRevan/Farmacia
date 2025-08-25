namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMVenta
    {
        public int IdVenta { get; set; }

        public string? Cliente { get; set; }

        public DateTime? FechaHoraVenta { get; set; }

        public decimal? TotalVenta  { get; set; }   

        public int? CantidadProductosVenta { get; set; }

        public int? IdUsuario { get; set; }

        public int? IdFormaPago { get; set; }

        public int? IdPrecioProdcuto { get; set; }

    }
}
