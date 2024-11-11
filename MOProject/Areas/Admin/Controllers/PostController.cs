using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;

using X.PagedList.Extensions;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            // Retrieve all posts (without any user-based filtering)
            var listOfPosts = await _context.Posts!
                .Include(x => x.ApplicationUser)  // You may still want to include the user for the author name
                .ToListAsync();

            // Project the list of posts into a list of ViewModels
            var listOfPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser.LastName
            }).AsQueryable();  // Convert to IQueryable to work with ToPagedList

            // Pagination logic
            int pageSize = 5;
            int pageNumber = (page ?? 1); // Default to page 1 if null

            // Apply ordering and paging
            var paginatedPosts = listOfPostsVM
                .OrderByDescending(x => x.CreatedDate)  // Sort by CreatedDate in descending order
                .ToPagedList(pageNumber, pageSize);    // Use ToPagedList for pagination

            // Return the paginated list to the view
            return View(paginatedPosts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (ModelState.IsValid) { return View(vm); }

            var post = new Post();

            post.Title = vm.Title!.Trim();
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;

            if (post.Title != null)
            {
                string slug = vm.Title.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("BlogPost");
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
