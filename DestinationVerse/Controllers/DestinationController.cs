using DestinationVerse.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // 2. GET екшън: Отваря празната форма и я подава към изгледа
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var regions = await regionService.GetAllRegionsAsync();

            var model = new ViewModels.Destination.DestinationFormModel()
            {
                Regions = regions.Select(r => new ViewModels.Destination.RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList()
            };

            return View("Create", model);
        }

        // 3. POST екшън: Приема данните от формата и ги записва в базата данни
        [HttpPost]
        public async Task<IActionResult> Create(ViewModels.Destination.DestinationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var regions = await regionService.GetAllRegionsAsync();
                model.Regions = regions.Select(r => new ViewModels.Destination.RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();

                return View("Create", model);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            var destination = new Data.Entities.Destination()
            {
                Title = model.Title,
                Description = model.Description,
                CoverImageUrl = model.CoverImageUrl,
                RegionId = model.RegionId,
                PublishedOn = DateTime.Now,
                PublisherId = userId,
                IsDeleted = false
            };

            await destinationService.AddDestinationAsync(destination);
            return RedirectToAction(nameof(Index));
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

        // 5. GET екшън: Зарежда формата за редактиране със съществуващите данни само ако потребителят е автор!
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            // СЪРВЪРНА ЗАЩИТА: Само авторът може да отвори тази форма
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (destination.PublisherId != userId)
            {
                return Unauthorized(); // Предотвратява неоторизиран достъп през URL адреса
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

        // 6. POST екшън: Записва редактираните данни само ако потребителят е автор!
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

            // СЪРВЪРНА ЗАЩИТА: Валидация на авторството преди запис в SQL Server
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (destination.PublisherId != userId)
            {
                return Unauthorized();
            }

            destination.Title = model.Title;
            destination.Description = model.Description;
            destination.CoverImageUrl = model.CoverImageUrl;
            destination.RegionId = model.RegionId;

            await destinationService.UpdateDestinationAsync(destination);
            return RedirectToAction(nameof(Index));
        }


        // 7. GET екшън: Извежда личната колекция на потребителя („Моят Пътеводител“)
        [HttpGet]
        public async Task<IActionResult> MyGuide()
        {
            // Взимаме уникалния Id идентификатор на текущо логнатия потребител по чист начин
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            var favorites = await destinationService.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        // 8. POST екшън: Добавя дестинация в колекцията и пренасочва обратно
        [HttpPost]
        public async Task<IActionResult> AddToGuide(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            await destinationService.AddToFavoritesAsync(userId, id);
            return RedirectToAction(nameof(MyGuide));
        }

        // 9. POST екшън: Премахва дестинация от колекцията
        [HttpPost]
        public async Task<IActionResult> RemoveFromGuide(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            await destinationService.RemoveFromFavoritesAsync(userId, id);
            return RedirectToAction(nameof(MyGuide));
        }

        // GET: Отваряне на формата
        [HttpGet]
        public IActionResult CreateRegion()
        {
            return View();
        }

        // POST: Само препраща данните към Services слоя
        [HttpPost]
        public async Task<IActionResult> CreateRegion(string name)
        {
            // Викаме бизнес логиката от сървиса
            bool isSuccess = await regionService.CreateRegionAsync(name);

            if (!isSuccess)
            {
                ModelState.AddModelError("Name", "Невалидно име или регионът вече съществува (между 3 и 20 символа).");
                return View();
            }

            // Връща ни обратно във формата за дестинация
            return RedirectToAction(nameof(Create));
        }

        // GET: Зарежда форма за редактиране на регион
        [HttpGet]
        public async Task<IActionResult> EditRegion(int id)
        {
            var region = await regionService.GetRegionByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            ViewBag.RegionId = id;
            return View("EditRegion", region.Name);
        }

        // POST: Записва редактирания регион чрез бизнес слоя
        [HttpPost]
        public async Task<IActionResult> EditRegion(int id, string name)
        {
            bool isSuccess = await regionService.UpdateRegionAsync(id, name);
            if (!isSuccess)
            {
                ModelState.AddModelError("Name", "Невалидно име на регион (между 3 and 20 символа).");
                ViewBag.RegionId = id;
                return View("EditRegion", name);
            }

            return RedirectToAction(nameof(Create));
        }

        // POST: Изтрива регион директно от списъка
        [HttpPost]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            await regionService.DeleteRegionAsync(id);
            return RedirectToAction(nameof(Create));
        }

        // ❌ 10. POST екшън: Сигурно изтриване (Soft Delete) на дестинация
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Извикваме бизнес логиката от сървиса (която задава IsDeleted = true)
            await destinationService.DeleteDestinationAsync(id);

            // Връщаме потребителя обратно на опреснения начален екран
            return RedirectToAction(nameof(Index));
        }
    }
}
