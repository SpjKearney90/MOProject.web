
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.ViewModels;

namespace MOProject.Views.Blog
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;


        public BlogController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("blog/post/{slug}")]
        public IActionResult Post(string slug)
        {
            if (slug == "")
            {

                return View();
            }
            var post = _context.Posts!.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Slug == slug);
            if (post == null)
            {

                return View();
            }
            var vm = new BlogPostVM()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser!.FirstName + " " + post.ApplicationUser.LastName,
                CreatedDate = post.CreatedDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
            };
            return View(vm);
        }
    }
}