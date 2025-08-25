namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMDevolucione
    {
        public int IdDevolucion { get; set; }

        public int? IdCompra { get; set; }

        public int? IdVenta { get; set; }

        public int? IdTipoDevolucion { get; set; }
    }
}
