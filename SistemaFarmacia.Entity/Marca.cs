using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string? NombreMarca { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
