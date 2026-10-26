using System.ComponentModel.DataAnnotations;

namespace DestinationVerse.ViewModels.Destination
{
    public class CommentFormModel
    {
        [Required(ErrorMessage = "Коментарът е задължителен.")]
        [StringLength(500, MinimumLength = 4, ErrorMessage = "Коментарът трябва да бъде между 4 и 500 символа.")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Оценката е задължителна.")]
        [Range(1, 5, ErrorMessage = "Оценката трябва да бъде между 1 и 5 звезди.")]
        public int Rating { get; set; }

        public int DestinationId { get; set; }
    }
}
