using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DestinationVerse.Data.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 4)]
        public string Text { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Оценка: от 1 до 5 звезди

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int DestinationId { get; set; }

        [ForeignKey(nameof(DestinationId))]
        public Destination Destination { get; set; } = null!;

        [Required]
        public string AuthorId { get; set; } = null!;

        [ForeignKey(nameof(AuthorId))]
        public IdentityUser Author { get; set; } = null!;
    }
}
