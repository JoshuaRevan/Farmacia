using Microsoft.AspNetCore.Mvc;

namespace SistemaFarmacia.AplicacionWed.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
