using DestinationVerse.Services.Contracts;
using DestinationVerse.ViewModels.Destination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DestinationVerse.Controllers
{
    [Authorize] // Задължително по задание: Само логнати потребители имат достъп до дестинациите
    public class DestinationController : Controller
    {
        private readonly IDestinationService destinationService;
        private readonly IRegionService regionService;
        private readonly ICommentService commentService;

        // IoC контейнерът инжектира трите услуги автоматично
        public DestinationController(
            IDestinationService destinationService,
            IRegionService regionService,
            ICommentService commentService)
        {
            this.destinationService = destinationService;
            this.regionService = regionService;
            this.commentService = commentService;
        }

        // 1. Страница „Всички дестинации“
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var destinations = await destinationService.GetAllAvailableDestinationsAsync();

            var ratings = new System.Collections.Generic.Dictionary<int, double>();
            foreach (var dest in destinations)
            {
                ratings[dest.Id] = await commentService.GetAverageRatingAsync(dest.Id);
            }
            ViewBag.Ratings = ratings;

            return View(destinations);
        }

        // 2. GET екшън: Отваря празната форма
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var regions = await regionService.GetAllRegionsAsync();

            var model = new DestinationFormModel()
            {
                Regions = regions.Select(r => new RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList()
            };

            return View("Create", model);
        }

        // 3. POST екшън: Приема данните от формата
        [HttpPost]
        public async Task<IActionResult> Create(DestinationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var regions = await regionService.GetAllRegionsAsync();
                model.Regions = regions.Select(r => new RegionSelectViewModel()
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

            ViewBag.Comments = await commentService.GetCommentsByDestinationIdAsync(id);
            ViewBag.AverageRating = await commentService.GetAverageRatingAsync(id);
            ViewBag.NewComment = new CommentFormModel { DestinationId = id };

            return View(destination);
        }

        //  4.1 POST екшън: Добавяне на коментар и оценка
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Коментарът трябва да бъде между 4 и 500 символа, а оценката между 1 и 5.";
                return RedirectToAction(nameof(Details), new { id = model.DestinationId });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var comment = new Data.Entities.Comment()
            {
                Text = model.Text.Trim(),
                Rating = model.Rating,
                DestinationId = model.DestinationId,
                AuthorId = userId
            };

            await commentService.AddCommentAsync(comment);
            TempData["SuccessMessage"] = "Успешно добавихте коментар и оценка!";
            return RedirectToAction(nameof(Details), new { id = model.DestinationId });
        }

        // 4.2 POST екшън: Изтриване на коментар
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id, int destinationId)
        {
            var comment = await commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            bool isAdmin = User.IsInRole("Administrator");

            if (comment.AuthorId == userId || isAdmin)
            {
                await commentService.DeleteCommentAsync(id);
                TempData["SuccessMessage"] = "Коментарът беше изтрит.";
            }
            else
            {
                TempData["ErrorMessage"] = "Нямате права да изтриете този коментар!";
            }

            return RedirectToAction(nameof(Details), new { id = destinationId });
        }

        // 5. GET екшън: Зарежда формата за редактиране
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                TempData["ErrorMessage"] = "Дестинацията не е намерена.";
                return RedirectToAction(nameof(Index));
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (destination.PublisherId != userId)
            {
                TempData["ErrorMessage"] = "Нямате права за редакция на чужда дестинация!";
                return RedirectToAction(nameof(Index));
            }

            var regions = await regionService.GetAllRegionsAsync();

            var model = new DestinationFormModel()
            {
                Title = destination.Title,
                Description = destination.Description,
                CoverImageUrl = destination.CoverImageUrl,
                RegionId = destination.RegionId,
                Regions = regions.Select(r => new RegionSelectViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                })
            };

            return View(model);
        }

        // 6. POST екшън: Записва редактираните данни
        [HttpPost]
        public async Task<IActionResult> Edit(int id, DestinationFormModel model)
        {
            if (!ModelState.IsValid)
            {
                var regions = await regionService.GetAllRegionsAsync();
                model.Regions = regions.Select(r => new RegionSelectViewModel() { Id = r.Id, Name = r.Name });
                return View(model);
            }

            var destination = await destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
            {
                TempData["ErrorMessage"] = "Дестинацията не е намерена.";
                return RedirectToAction(nameof(Index));
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (destination.PublisherId != userId)
            {
                TempData["ErrorMessage"] = "Нямате права за модифициране на чужда дестинация!";
                return RedirectToAction(nameof(Index));
            }

            destination.Title = model.Title;
            destination.Description = model.Description;
            destination.CoverImageUrl = model.CoverImageUrl;
            destination.RegionId = model.RegionId;

            await destinationService.UpdateDestinationAsync(destination);
            TempData["SuccessMessage"] = "Дестинацията е обновена успешно.";
            return RedirectToAction(nameof(Index));
        }

        // 7. GET екшън: Извежда личната колекция на потребителя („Моят Пътеводител“)
        [HttpGet]
        public IActionResult MyGuide()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            var favorites = destinationService.GetUserFavoritesAsync(userId).GetAwaiter().GetResult();
            return View("MyGuide", favorites);
        }

        // 8. POST екшън: Добавя дестинация в колекцията
        [HttpPost]
        public IActionResult AddToGuide(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            destinationService.AddToFavoritesAsync(userId, id).GetAwaiter().GetResult();
            return RedirectToAction(nameof(MyGuide));
        }

        // 9. POST екшън: Премахва дестинация от колекцията
        [HttpPost]
        public IActionResult RemoveFromGuide(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException("Потребителят не е намерен.");

            destinationService.RemoveFromFavoritesAsync(userId, id).GetAwaiter().GetResult();
            return RedirectToAction(nameof(MyGuide));
        }
        // GET: Отваряне на формата за нов регион
        [HttpGet]
        public IActionResult CreateRegion()
        {
            return View();
        }
        // POST: Само препраща данните към Services слоя
        [HttpPost]
        public IActionResult CreateRegion(string name)
        {
            bool isSuccess = regionService.CreateRegionAsync(name).GetAwaiter().GetResult();
            if (!isSuccess)
            {
                ModelState.AddModelError("Name", "Невалидно име или регионът вече съществува (между 3 и 20 символа).");
                return View();
            }
            return RedirectToAction(nameof(Create));
        }
        // GET: Зарежда форма за редактиране на регион
        [HttpGet]
        public IActionResult EditRegion(int id)
        {
            var region = regionService.GetRegionByIdAsync(id).GetAwaiter().GetResult();
            if (region == null)
            {
                return NotFound();
            }
            ViewBag.RegionId = id;
            return View("EditRegion", region.Name);
        }
        // POST: Записва редактирания регион чрез бизнес слоя
        [HttpPost]
        public IActionResult EditRegion(int id, string name)
        {
            bool isSuccess = regionService.UpdateRegionAsync(id, name).GetAwaiter().GetResult();
            if (!isSuccess)
            {
                ModelState.AddModelError("Name", "Невалидно име на регион (между 3 и 20 символа).");
                ViewBag.RegionId = id;
                return View("EditRegion", name);
            }
            return RedirectToAction(nameof(Create));
        }
        // POST: Изтрива регион директно от списъка
        [HttpPost]
        public IActionResult DeleteRegion(int id)
        {
            regionService.DeleteRegionAsync(id).GetAwaiter().GetResult();
            return RedirectToAction(nameof(Create));
        }
        // 10. POST екшън: Сигурно изтриване (Soft Delete) на дестинация
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var destination = destinationService.GetDestinationByIdAsync(id).GetAwaiter().GetResult();
            if (destination == null)
            {
                TempData["ErrorMessage"] = "Дестинацията не е намерена.";
                return RedirectToAction(nameof(Index));
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (destination.PublisherId != userId)
            {
                TempData["ErrorMessage"] = "Нямате права за изтриване на чужда дестинация!";
                return RedirectToAction(nameof(Index));
            }
            destinationService.DeleteDestinationAsync(id).GetAwaiter().GetResult();
            TempData["SuccessMessage"] = "Дестинацията е изтрита успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}