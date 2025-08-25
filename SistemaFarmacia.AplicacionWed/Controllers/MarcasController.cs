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
    public class MarcasController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMarcaService _marcaService;

        public MarcasController(IMapper mapper, IMarcaService marcaService)
        {
            _mapper = mapper;
            _marcaService = marcaService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMMarca> vmMarcaLista = _mapper.Map<List<VMMarca>>(await _marcaService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmMarcaLista });

        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMMarca modelo)
        {
            GenericResponse<VMMarca> gResponse = new GenericResponse<VMMarca>();

            try
            {
                Marca marca_creada = await _marcaService.Crear(_mapper.Map<Marca>(modelo));
                modelo = _mapper.Map<VMMarca>(marca_creada);

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
        public async Task<IActionResult> Editar([FromBody] VMMarca modelo)
        {
            GenericResponse<VMMarca> gResponse = new GenericResponse<VMMarca>();

            try
            {
                Marca marca_editada = await _marcaService.Editar(_mapper.Map<Marca>(modelo));
                modelo = _mapper.Map<VMMarca>(marca_editada);

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
        public async Task<IActionResult> Eliminar(int idMarca)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.Estado = await _marcaService.Eliminar(idMarca);
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
        
    }
}
