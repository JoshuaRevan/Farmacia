using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class PrecioProducto
{
    public int IdPrecioProducto { get; set; }
    //Precio se refiere al Precio de venta al publico este Campo se debe visualizar en Productos
    public decimal? Precio { get; set; }

    public int? IdProducto { get; set; }

    public int? IdProveedor { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual Proveedore? IdProveedorNavigation { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
