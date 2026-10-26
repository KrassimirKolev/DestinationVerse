using DestinationVerse.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DestinationVerse.Controllers
{
    [Authorize] // Задължително по задание: Само логнати потребители имат достъп до дестинациите
    public class DestinationController : Controller
    {
        private readonly IDestinationService destinationService;
        private readonly IRegionService regionService;

        // Вграденият в .NET IoC контейнер автоматично инжектира нашите CRUD услуги тук
        public DestinationController(IDestinationService destinationService, IRegionService regionService)
        {
            this.destinationService = destinationService;
            this.regionService = regionService;
        }

        // 1. Страница „Всички дестинации“ (Index / All)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var destinations = await destinationService.GetAllAvailableDestinationsAsync();
            return View(destinations);
        }
    }
}
