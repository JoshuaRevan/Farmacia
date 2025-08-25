using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string? NombreProveedor { get; set; }

    public string? TelefonoProveedor { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<PrecioProducto> PrecioProductos { get; set; } = new List<PrecioProducto>();
}
