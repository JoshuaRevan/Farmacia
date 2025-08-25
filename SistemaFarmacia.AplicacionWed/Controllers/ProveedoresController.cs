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
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProveedorService _proveedorService;

        public ProveedoresController(IMapper mapper, IProveedorService proveedorService)
        {
            _mapper = mapper;
            _proveedorService = proveedorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMProveedores> vmProveedores = _mapper.Map<List<VMProveedores>>(await _proveedorService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmProveedores });

        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMProveedores modelo)
        {
            GenericResponse<VMProveedores> gResponse = new GenericResponse<VMProveedores>();

            try
            {
                Proveedore proveedore_creado = await _proveedorService.Crear(_mapper.Map<Proveedore>(modelo));
                modelo = _mapper.Map<VMProveedores>(proveedore_creado);

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
        public async Task<IActionResult> Editar([FromBody] VMProveedores modelo)
        {
            GenericResponse<VMProveedores> gResponse = new GenericResponse<VMProveedores>();

            try
            {
                Proveedore proveedore_editado = await _proveedorService.Editar(_mapper.Map<Proveedore>(modelo));
                modelo = _mapper.Map<VMProveedores>(proveedore_editado);

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

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idProveedor)
        {
            GenericResponse<String> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.Estado = await _proveedorService.Eliminar(idProveedor);

            }catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;

            }
            return StatusCode(StatusCodes.Status200OK, gResponse);   
        }
    }
}
