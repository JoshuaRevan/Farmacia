using SistemaFarmacia.Entity;

namespace SistemaFarmacia.AplicacionWed.Models.ViewModels
{
    public class VMUsuario
    {
        public int IdUsuario { get; set; }

        public string? NombreUsuario { get; set; }

        public string? ApellidoUsuario { get; set; }

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public int? IdCargo { get; set; }

        public string? NombreCargo { get; set; }
       
        public string? Contrasena { get; set; }

    }
}
