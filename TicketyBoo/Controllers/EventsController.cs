using Microsoft.AspNetCore.Mvc;

namespace TicketyBoo.Controllers
{
    public class EventsController : Controller
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
