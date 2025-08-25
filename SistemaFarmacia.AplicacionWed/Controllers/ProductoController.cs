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
using SistemaFarmacia.BLL.Implementacion;


namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductoService _productoService;

        public ProductoController(IMapper mapper, IProductoService productoService)
        {
            _mapper = mapper;
            _productoService = productoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMProducto> vmProductoLista = _mapper.Map<List<VMProducto>>(await _productoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new {data = vmProductoLista});
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMProducto modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();

            try
            {
                Producto producto_creado = await _productoService.Crear(_mapper.Map<Producto>(modelo));
                modelo = _mapper.Map<VMProducto>(producto_creado);

                gResponse.Estado = true;
                gResponse.objeto = modelo;
            }
            catch (Exception ex) 
            { 
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMProducto modelo)
        {
            GenericResponse<VMProducto> gResponse = new GenericResponse<VMProducto>();

            try
            {
                Producto producto_editado = await _productoService.Editar(_mapper.Map<Producto>(modelo));
                modelo = _mapper.Map<VMProducto>(producto_editado);

                gResponse.Estado = true;    
                gResponse.objeto = modelo;
            }
            catch (Exception ex) 
            {
                gResponse.Estado = false;
                gResponse.Mensaje= ex.Message;

            }

            return StatusCode(StatusCodes.Status200OK, gResponse );
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.Estado = await _productoService.Eliminar(idProducto);

            }
            catch (Exception ex) 
            { 
                gResponse.Estado = false ;
                gResponse.Mensaje= ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
