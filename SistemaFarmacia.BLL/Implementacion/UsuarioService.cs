using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaFarmacia.BLL.Interfaces;
using SistemaFarmacia.DAL.Interfaces;
using SistemaFarmacia.DAL.Implementacion;
using SistemaFarmacia.Entity;
using Microsoft.EntityFrameworkCore;

namespace SistemaFarmacia.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;

        private readonly IUtilidadesService _utilidadesService;

        public  UsuarioService(IGenericRepository <Usuario> usuarioRepositorio, IUtilidadesService IUtilidadesService)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _utilidadesService = IUtilidadesService;
        }

        public async Task<Usuario> ValidarCrendenciales(string correo, string contrasena)
        {
            var usuario = await _usuarioRepositorio.Consultar(u => u.Correo == correo);
            var usuarioEncontrado = usuario.FirstOrDefault();

            if (usuarioEncontrado == null)
                return null;

            bool claveValida = _utilidadesService.VerificarClave(contrasena, usuarioEncontrado.Contrasena);

            return claveValida ? usuarioEncontrado : null; 
        }

        public async Task<string> EncriptarClave(string contrasena)
        {
            return _utilidadesService.EncriptarClave(contrasena);
        }

        public async Task<Usuario> Crear(Usuario modelo)
        {
            string claveGenerada = _utilidadesService.GenerarClave();
            modelo.Contrasena = _utilidadesService.EncriptarClave(claveGenerada);

            Usuario usuarioCreado = await _usuarioRepositorio.Crear(modelo);

            Usuario usuarioconDatos = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioCreado.IdUsuario, "IdCargoNavigation");

            usuarioconDatos.Contrasena = claveGenerada; // Para mostrar la clave generada al admin, sin devolverla encriptada

            return usuarioCreado;
        }
        public async Task<bool> CambiarContrasenaAsync(int idUsuario, string contrasenaActual, string nuevaContrasena)
        {
            var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == idUsuario);
            
            if (usuario == null) 
                return false;

            bool claveCorrecta = _utilidadesService.VerificarClave(contrasenaActual, usuario.Contrasena);
            if (!claveCorrecta)
                return false;

            usuario.Contrasena = _utilidadesService.EncriptarClave(nuevaContrasena);

            return await _usuarioRepositorio.Editar(usuario);
        }

        public async Task<List<Usuario>> Listar()
        {
            IQueryable<Usuario> query = await _usuarioRepositorio.Consultar();
            return await query.ToListAsync();

        }

        public async Task<Usuario> ObtenerporId(int id)
        {
            return await _usuarioRepositorio.Obtener(u =>u.IdUsuario == id);
        }

        public async Task<bool> Editar(Usuario modelo)
        {
            return await _usuarioRepositorio.Editar(modelo);

        }

        public async Task<bool> Editar(int id)
        {
            var usuario = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
            if(usuario == null) 
              return false;

            return await _usuarioRepositorio.Eliminar(usuario);
        }

        public async Task<bool> Eliminar(int id)
        {
            var usuario = await _usuarioRepositorio.Obtener(u =>u.IdUsuario == id);
            if (usuario == null)
                return false;

            return await _usuarioRepositorio.Eliminar(usuario);
        }

    }
}
