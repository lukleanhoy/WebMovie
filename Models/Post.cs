using System;
using System.ComponentModel.DataAnnotations;

namespace WebMovie.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Image URL")]
        [Required(ErrorMessage = "Image URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1888, 2100, ErrorMessage = "Please enter a valid release year.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(0.0, 10.0, ErrorMessage = "Rating must be between 0.0 and 10.0.")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;

        public int? UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
