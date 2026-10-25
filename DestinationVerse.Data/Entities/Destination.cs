using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DestinationVerse.Data.Entities
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        public string? CoverImageUrl { get; set; } // Опционално (Nullable) поле по задание

        [Required]
        public string PublisherId { get; set; } = null!;

        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!; // Връзка към системния потребител на Identity

        [Required]
        public DateTime PublishedOn { get; set; }

        [Required]
        public int RegionId { get; set; }

        [ForeignKey(nameof(RegionId))]
        public Region Region { get; set; } = null!; // Връзка към региона

        public bool IsDeleted { get; set; } = false; // Задължителната Soft Delete поддръжка

        public ICollection<UserDestination> UsersDestinations { get; set; } = new HashSet<UserDestination>();
    }
}
