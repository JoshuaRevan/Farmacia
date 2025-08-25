using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class ReporteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
