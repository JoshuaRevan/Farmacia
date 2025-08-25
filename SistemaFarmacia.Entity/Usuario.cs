using System;
using System.Collections.Generic;

namespace SistemaFarmacia.Entity;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ApellidoUsuario { get; set; }

    public string? Contrasena { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public int? IdCargo { get; set; }

    public virtual Cargo? IdCargoNavigation { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
