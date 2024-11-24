using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor injection
        public PostController(ApplicationDbContext context,
                              IWebHostEnvironment webHostEnvironment,
                              UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Dash(int? page)
        {
            var listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).ToListAsync();
            var listOfPostsVM = listOfPosts.Select(x => new PostVM
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser.LastName
            }).ToList();

            int pageSize = 5;
            int pageNumber = page ?? 1;

            return View(listOfPostsVM.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                // Logging errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(vm);
            }

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            if (loggedInUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(vm);
            }

            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                ApplicationUserId = loggedInUser.Id
            };

            // Generate slug
            if (!string.IsNullOrWhiteSpace(post.Title))
            {
                string slug = vm.Title!.Trim().Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dash", "User", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dash", "User", new { area = "Admin" });
            }

            ModelState.AddModelError("", "Post not found.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                ModelState.AddModelError("", "Post not found.");
                return View();
            }

            var vm = new CreatePostVM
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(vm);
            }

            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
                ModelState.AddModelError("", "Post not found.");
                return View();
            }

            post.Title = vm.Title;
            post.ShortDescription = vm.ShortDescription;
            post.Description = vm.Description;

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Dash", "User", new { area = "Admin" });
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            var filePath = Path.Combine(folderPath, uniqueFileName);
            Directory.CreateDirectory(folderPath); // Ensure the directory exists

            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}