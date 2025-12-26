using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Models
{
    public class CompraListaDto
    {
        public int IdCompra { get; set; }

        //Producto
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public int IdPresentacion { get; set; }
        public string? TipoPresentacion { get; set; }
        public int IdMarca { get; set; }
        public string? NombreMarca { get; set; }

        //Proveedor
        public int IdProveedor { get; set; }
        public string? NombreProveedor { get; set; }

        //Compra
        public decimal? TotalCompra { get; set; } //precio al que se compro el producto
        public int? CantidadProductosCompra { get; set; } //Productos que entran al sistema
        public int? LoteInterno { get; set; }
        public int? LoteProveedor { get; set; }
        public DateTime? FechaHoraCompra { get; set; }
        public DateTime? fechaVencimiento { get; set; }

        //Precio
       // public int IdPrecioProducto { get; set; }
       // public decimal? Precio { get; set; } //Precio de venta la publico

    }
}
