using System.ComponentModel.DataAnnotations;

namespace MOProject.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The short description is required.")]
        public string? ShortDescription { get; set; }

        // relation
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "The description is required.")]
        public string? Description { get; set; }

        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
