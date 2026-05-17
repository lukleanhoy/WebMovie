using System;
using System.ComponentModel.DataAnnotations;

namespace WebMovie.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie Title is required.")]
        [Display(Name = "Movie Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Poster Path")]
        public string PosterPath { get; set; } = string.Empty;

        [Display(Name = "Thumbnail Path")]
        public string ThumbnailPath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "End Episode is required.")]
        [Display(Name = "End Episode")]
        public int EndEpisode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
