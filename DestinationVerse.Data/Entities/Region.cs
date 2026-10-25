using System.ComponentModel.DataAnnotations;

namespace DestinationVerse.Data.Entities
{
    public class Region
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        // Навигационно свойство: Един регион ще съдържа много дестинации
        public ICollection<Destination> Destinations { get; set; } = new HashSet<Destination>();
    }
}
