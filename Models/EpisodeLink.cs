using System;
using System.ComponentModel.DataAnnotations;

namespace WebMovie.Models
{
    public class EpisodeLink
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie selection is required.")]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [Required(ErrorMessage = "Movie Link is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Link { get; set; } = string.Empty;

        [Required(ErrorMessage = "Episode number is required.")]
        [Range(1, 10000, ErrorMessage = "Episode number must be positive.")]
        public int Episode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
