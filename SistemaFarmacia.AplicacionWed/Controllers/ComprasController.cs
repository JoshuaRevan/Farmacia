using Microsoft.AspNetCore.Mvc;

using SistemaFarmacia.BLL.Models;
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
    public class ComprasController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompraService _compraService;
        public ComprasController(IMapper mapper, ICompraService compraService) 
        {
            _mapper = mapper;
            _compraService = compraService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<CompraListaDto> compraDtos = await _compraService.Lista();
            return Ok(new {data = compraDtos}); 
        }   
    }
}
