using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface IUtilidadesService
    {
        string GenerarClave();

        string EncriptarClave(string Contrasena);

        bool VerificarClave(string ContrasenaIngresada, string hashAlmacenado);
    }
}
