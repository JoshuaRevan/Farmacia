using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Transaccione
{
    public int IdTransaccion { get; set; }

    public string? TipoTransaccion { get; set; }
    //en el caso de Catidad hace referencia al stock de un producto 
    public int? Cantidad { get; set; }

    public DateTime? FechaHoraTransaccion { get; set; }

    public int? IdProducto { get; set; }

    public int? IdCompra { get; set; }

    public int? IdVenta { get; set; }

    public int? IdDevolucion { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Devolucione? IdDevolucionNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
