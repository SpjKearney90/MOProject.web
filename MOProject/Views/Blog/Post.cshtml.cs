using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.ViewModels;

public class BlogController : Controller
{
    private readonly ApplicationDbContext _context;

    public BlogController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/Post/{slug}")]
    public IActionResult Post(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var post = _context.Posts
            .Include(p => p.ApplicationUser)
            .FirstOrDefault(p => p.Slug == slug);

        if (post == null)
        {
            return NotFound();
        }

        var vm = new BlogPostVM
        {
            Title = post.Title,
            ShortDescription = post.ShortDescription,
            AuthorName = $"{post.ApplicationUser.FirstName} {post.ApplicationUser.LastName}",
            CreatedDate = post.CreatedDate,
            ThumbnailUrl = post.ThumbnailUrl,
            Description = post.Description
        };

        return View(vm); // Pass the ViewModel to the view
    }
}
