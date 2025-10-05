using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketyBoo.Models;
using TicketyBoo.Data; // Add this for your context
using System.Linq;    // For ToList()

namespace TicketyBoo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TicketyBooContext _context; // Add this

        public HomeController(ILogger<HomeController> logger, TicketyBooContext context) // Add context to constructor
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var haunts = _context.Haunt.ToList(); // Fetch from database
            return View(haunts);
        }

        public IActionResult Details()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
