using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Models
{
    public class CompraRequestDto
    {
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public int IdPresentacion { get; set; }
        public int IdMarca { get; set; }
        public int IdProveedor { get; set; }
        public int? CantidadProductosCompra { get; set; } //Productos que entran al sistema
        public decimal? TotalCompra { get; set; } //precio al que se compro el producto
       // public int IdPrecioProducto { get; set; }
        public decimal? Precio { get; set; } //Precio de venta asignado
        public int? LoteInterno { get; set; }
        public int? LoteProveedor { get; set; }
        public DateTime? FechaHoraCompra { get; set; }
        public DateTime? fechaVencimiento { get; set; }

    }
}
