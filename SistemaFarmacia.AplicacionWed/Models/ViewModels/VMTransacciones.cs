namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMTransacciones
    {
        public int IdTransaccion { get; set; }

        public string? TipoTransaccion { get; set; }

        public int? Cantidad { get; set; }

        public DateTime? FechaHoraTransaccion { get; set; }

        public int? IdProducto { get; set; }

        public int? IdCompra { get; set; }

        public int? IdVenta { get; set; }

        public int? IdDevolucion { get; set; }
    }
}
