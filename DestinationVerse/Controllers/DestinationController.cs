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

        // 4. GET екшън: Детайли за конкретна дестинация
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // 5. GET екшън: Зарежда формата за редактиране със съществуващите данни
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            var regions = await regionService.GetAllRegionsAsync();

            var model = new ViewModels.Destination.DestinationFormModel()
            {
                Title = destination.Title,
                Description = destination.Description,
                CoverImageUrl = destination.CoverImageUrl,
                RegionId = destination.RegionId,
                Regions = regions.Select(r => new ViewModels.Destination.RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                })
            };

            return View(model);
        }

        // 6. POST екшън: Записва редактираните данни в SQL базата данни
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ViewModels.Destination.DestinationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var regions = await regionService.GetAllRegionsAsync();
                model.Regions = regions.Select(r => new ViewModels.Destination.RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                });
                return View(model);
            }

            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            destination.Title = model.Title;
            destination.Description = model.Description;
            destination.CoverImageUrl = model.CoverImageUrl;
            destination.RegionId = model.RegionId;

            await destinationService.UpdateDestinationAsync(destination);
            return RedirectToAction(nameof(Index));
        }

    }
}
