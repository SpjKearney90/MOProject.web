using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;

using X.PagedList.Extensions;

namespace FineBlog.Areas.Admin.Controllers
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
            // Fetching all posts from the database since we assume there is only one admin
            var listOfPosts = await _context.Posts!.Include(x => x.ApplicationUser).ToListAsync();

            // Transforming posts into ViewModel objects to pass to the view
            var listOfPostsVM = listOfPosts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = x.ApplicationUser!.FirstName + " " + x.ApplicationUser.LastName
            }).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

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
                return View(vm);
            }

            // Assuming the logged-in user is the admin
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                ApplicationUserId = loggedInUser!.Id
            };

            // Generate slug
            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            // Handle thumbnail image upload if provided
            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            // Add the post to the database
            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("/Admin/User/dash");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);

            // Assuming the logged-in user is the admin (no role checks needed)
            if (post != null)
            {
                _context.Posts!.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dash", "User", new { area = "Admin" });
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                return View();
            }

            // Mapping the existing post to the Edit view model
            var vm = new CreatePostVM()
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                ThumbnailUrl = post.ThumbnailUrl,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var post = await _context.Posts!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (post == null)
            {
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
