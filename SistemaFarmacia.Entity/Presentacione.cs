using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Presentacione
{
    public int IdPresentacion { get; set; }

    public string? TipoPresentacion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
