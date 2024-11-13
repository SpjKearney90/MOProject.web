using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MOProject.ViewModels
{
    public class CreatePostVM


    {

        public int Id { get; set; }  
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Short description is required.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public string? ThumbnailUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Thumbnail { get; set; }
    }
}
