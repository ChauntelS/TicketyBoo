using Microsoft.AspNetCore.Mvc;

namespace TicketyBoo.Controllers
{
    public class HauntsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
