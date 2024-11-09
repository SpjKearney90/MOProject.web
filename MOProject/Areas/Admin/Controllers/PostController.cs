using Microsoft.AspNetCore.Mvc;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

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

        public IActionResult BlogPost()
        {
            return View();
        }

        // GET: Create new Post
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        // POST: Create new Post
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Create a new Post object
            var post = new Post
            {
                Title = vm.Title!.Trim(),
                Description = vm.Description,
                ShortDescription = vm.ShortDescription
            };

            // Generate slug from the title
            if (!string.IsNullOrEmpty(post.Title))
            {
                string slug = GenerateSlug(post.Title);
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            // Handle the thumbnail image upload
            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            // Add the post to the database and save changes
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Redirect to the dashboard or another page
            return RedirectToAction("Dash", "Dashboard");  // Make sure "Dash" action exists
        }

        // Helper method to handle image upload
        private string UploadImage(IFormFile file)
        {
            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Define the path to store the file in wwwroot/thumbnails folder
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        // Helper method to generate a slug from the title
        private string GenerateSlug(string title)
        {
            // Clean up the title for use in a URL
            var slug = string.Join("-", title.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .ToLower()
                .Trim();
            return slug;
        }
    }
}
