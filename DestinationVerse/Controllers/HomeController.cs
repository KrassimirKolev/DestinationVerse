using DestinationVerse.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DestinationVerse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Ако потребителят е влязъл в профила си, Home автоматично го праща директно при дестинациите
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Destination");
            }

            // Ако е гост, вижда стандартната начална заглавна страница
            return View();
        }


        public IActionResult Privacy()
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
