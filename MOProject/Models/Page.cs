using System.ComponentModel.DataAnnotations;

namespace MOProject.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The short description is required.")]
        public string? ShortDescription { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string? Description { get; set; }

        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
