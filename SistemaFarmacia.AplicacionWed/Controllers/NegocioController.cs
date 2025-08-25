using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
