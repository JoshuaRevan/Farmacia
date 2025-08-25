using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Cargo
{
    public int IdCargo { get; set; }

    public string? cargo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
