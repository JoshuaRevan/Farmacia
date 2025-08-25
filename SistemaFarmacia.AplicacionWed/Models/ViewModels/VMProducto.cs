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

        //Campos solo para visualizacion
        //Cantidad traida de la tabla Transacciones, Cantidad revela el stcok de un producto
        public int? Cantidad { get; set; }

        //Precio traido de la tabla precioProducto por que cada producto tiene un precio diferente aunque sean de igaules marcas o direntes presentacion 
        public decimal? Precio { get; set; }
        //fechavencimiento traido de la tabla compra para darle un seguimento al producto
        public DateOnly? FechaVencimiento { get; set; }
        //LoterInterno traido de la tabla compra  
        public int? LoteInterno { get; set; }
    }
}
