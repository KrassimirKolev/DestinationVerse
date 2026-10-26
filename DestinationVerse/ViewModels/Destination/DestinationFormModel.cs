using System.ComponentModel.DataAnnotations;

namespace DestinationVerse.ViewModels.Destination
{
    public class DestinationFormModel
    {
        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Заглавието трябва да бъде между 3 и 80 символа.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Описанието е задължително.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "Описанието трябва да бъде между 10 и 250 символа.")]
        public string Description { get; set; } = null!;

        [Display(Name = "Линк към снимка")]
        public string? CoverImageUrl { get; set; }

        [Required(ErrorMessage = "Регионът е задължителен.")]
        [Display(Name = "Регион")]
        public int RegionId { get; set; }

        // Падащото меню ще се пълни през този списък, за да може потребителят да избира регион
        public IEnumerable<RegionSelectViewModel> Regions { get; set; } = new List<RegionSelectViewModel>();
    }
}
