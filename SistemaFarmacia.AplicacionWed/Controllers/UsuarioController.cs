using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Newtonsoft.Json;
using SistemaFarmacia.AplicacionWed.Models.ViewModels;
using SistemaFarmacia.AplicacionWed.Utilidades.Response;
using SistemaFarmacia.BLL.Interfaces;
using SistemaFarmacia.Entity;
using sistemaFarmacia.DAL.Implementacion;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.DAL.DBContext;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICargoService _cargoService;
        private readonly IMapper _mapper;
        private readonly FarmaciaTroyaContext _context;

        public UsuarioController(IUsuarioService usuarioService, ICargoService cargoService, IMapper mapper, FarmaciaTroyaContext context)
        {
            _usuarioService = usuarioService;
            _cargoService = cargoService;
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaCargo()
        {
            var listaCargos = await _cargoService.Listar();
            var VMcargo = _mapper.Map<List<VMcargo>>(listaCargos);
            return StatusCode(StatusCodes.Status200OK, new { data = VMcargo });//este es el metodo que me devuelve la lsita de Cargo en json
        }


        [HttpGet]
        public async Task<IActionResult> Lista()
        {
          var usuarios = await _context.Usuarios
                .Include(u => u.IdCargoNavigation)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.NombreUsuario,
                    u.ApellidoUsuario,
                    u.Telefono,
                    u.Correo,
                    u.IdCargo,
                    NombreCargo = u.IdCargoNavigation.cargo
                })
                .ToListAsync();
            return Ok(new { data = usuarios });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();
            try
            {
                Usuario nuevoUsuario = new Usuario
                {
                    NombreUsuario = modelo.NombreUsuario,
                    ApellidoUsuario = modelo.ApellidoUsuario,
                    Telefono = modelo.Telefono,
                    Correo = modelo.Correo,
                    IdCargo = modelo.IdCargo
                };

                Usuario usuarioCreado = await _usuarioService.Crear(nuevoUsuario);

                VMUsuario vm = new VMUsuario
                {
                    IdUsuario = usuarioCreado.IdUsuario,
                    NombreUsuario = usuarioCreado.NombreUsuario,
                    ApellidoUsuario = usuarioCreado.ApellidoUsuario,
                    Telefono = usuarioCreado.Telefono,
                    Correo = usuarioCreado.Correo,
                    IdCargo = usuarioCreado.IdCargo,
                    NombreCargo = usuarioCreado.IdCargoNavigation?.cargo ?? "Sin Cargo",
                    Contrasena = usuarioCreado.Contrasena // ← esta es la clave generada (sin encriptar)
                };

                gResponse.Estado = true;
                gResponse.objeto = vm;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;

            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> gResponse = new GenericResponse<VMUsuario>();
            try
            {
                bool resultado = await _usuarioService.Editar(_mapper.Map<Usuario>(modelo));

                gResponse.Estado = resultado;

                if(resultado)
                {
                    gResponse.objeto = modelo;
                }
                else
                {
                    gResponse.Mensaje = "No se pudoo editar el usuario";
                }
            }
            catch(Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message; 
            }

            return StatusCode(StatusCodes.Status200OK, gResponse );
        }
        

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Estado = await _usuarioService.Eliminar(idUsuario);
            }
            catch(Exception ex) 
            { 
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        
        

    }
}
