using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Devolucione
{
    public int IdDevolucion { get; set; }

    public int? IdCompra { get; set; }

    public int? IdVenta { get; set; }

    public int? IdTipoDevolucion { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual TipoDevolucione? IdTipoDevolucionNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
