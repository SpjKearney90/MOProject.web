using MOProject.Models;
using X.PagedList;

namespace MOProject.ViewModels
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IPagedList<Post>? Posts { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

    }
}


