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
    public class PresentacionesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPresentacionService  _presentacionService;

        public PresentacionesController(IMapper mapper, IPresentacionService presentacionService)
        {
            _mapper = mapper;
            _presentacionService = presentacionService;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMPresentacione> vmPresentacionLista = _mapper.Map<List<VMPresentacione>>(await _presentacionService.Lista());
            return StatusCode(StatusCodes.Status200OK, new {data = vmPresentacionLista});

        }
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMPresentacione modelo)
        {
            GenericResponse<VMPresentacione> gResponse = new GenericResponse<VMPresentacione>();

            try
            {
                Presentacione presentacione_creada = await _presentacionService.Crear(_mapper.Map<Presentacione>(modelo));
                modelo = _mapper.Map<VMPresentacione>(presentacione_creada);

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
        public async Task<IActionResult> Editar([FromBody] VMPresentacione modelo)
        {
            GenericResponse<VMPresentacione> gResponse = new GenericResponse<VMPresentacione>();

            try
            {
                Presentacione presentacione_editada = await _presentacionService.Editar(_mapper.Map<Presentacione>(modelo));
                modelo = _mapper.Map<VMPresentacione>(presentacione_editada);

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
        public async Task<IActionResult> ELiminar(int idPresentacion)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();
            try
            {
                gResponse.Estado = await _presentacionService.Eliminar(idPresentacion);
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
