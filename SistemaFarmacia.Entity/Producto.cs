using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? NombreProducto { get; set; }

    public string? Descripcion { get; set; }

    public string? Ubicacion { get; set; }

    public int? IdMarca { get; set; }

    public int? IdPresentacion { get; set; }

    public virtual Marca? IdMarcaNavigation { get; set; }

    public virtual Presentacione? IdPresentacionNavigation { get; set; }

    public virtual ICollection<PrecioProducto> PrecioProductos { get; set; } = new List<PrecioProducto>();

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
