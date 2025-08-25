using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Formaspago
{
    public int IdFormPago { get; set; }

    public string? FormPago { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
