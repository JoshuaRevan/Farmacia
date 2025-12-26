using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateTime? FechaHoraCompra { get; set; }

    public decimal? TotalCompra { get; set; }

    // public int IdPrecioProducto { get; set; }

    public int IdProducto { get; set; }

    public int? CantidadProductosCompra { get; set; }

    public int IdProveedor { get; set; }

    public int? LoteInterno { get; set; }

    public int? LoteProveedor { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();

    public virtual Proveedore? IdProveedorNavigation { get; set; }
    // public virtual PrecioProducto IdPrecioProductoNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
