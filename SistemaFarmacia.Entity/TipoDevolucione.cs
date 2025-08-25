using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class TipoDevolucione
{
    public int IdTipoDevolucion { get; set; }

    public string? TipoDevolucion { get; set; }

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();
}
