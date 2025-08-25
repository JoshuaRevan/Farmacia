using SistemaFarmacia.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaFarmacia.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ValidarCrendenciales(string correo, string contrasena);

        Task<string> EncriptarClave(string contrasena);

        Task<Usuario> Crear(Usuario modelo);

        Task<bool> CambiarContrasenaAsync(int IdUsuario, string contrasenaActual, string nuevaContrasena);

        Task<List<Usuario>> Listar();

        Task<Usuario> ObtenerporId(int id);

        Task<bool> Editar(Usuario modelo);

        Task<bool> Eliminar(int id); 
    }
}
