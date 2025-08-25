using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class ComprasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
