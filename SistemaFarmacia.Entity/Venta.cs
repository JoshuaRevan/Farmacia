using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Venta
{
    public int IdVenta { get; set; }

    public string? Cliente { get; set; }

    public DateTime? FechaHoraVenta { get; set; }

    public decimal? TotalVenta { get; set; }

    public int? CantidadProductosVenta { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdFormPago { get; set; }

    public int? IdPrecioProducto { get; set; }

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();

    public virtual Formaspago? IdFormPagoNavigation { get; set; }

    public virtual PrecioProducto? IdPrecioProductoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
