using Microsoft.AspNetCore.Mvc;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using X.PagedList;
using Microsoft.EntityFrameworkCore;


namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]  // This indicates that the controller is in the Admin area
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Blog Index Action
        public async Task<IActionResult> Index(int? page)
        {
            var vm = new HomeVM();

            // Fetch settings asynchronously
            var setting = await _context.Settings!.FirstOrDefaultAsync();
            if (setting != null)
            {
                vm.Title = setting.Title;
                vm.ShortDescription = setting.ShortDescription;
                vm.ThumbnailUrl = setting.ThumbnailUrl;
            }

            // Pagination setup
            int pageSize = 4; // Number of posts per page
            int pageNumber = page ?? 1; // Default to page 1 if not provided

            // Query posts and apply pagination
            var postsQuery = _context.Posts!
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.CreatedDate);

            // Get the posts for the current page asynchronously
            var posts = await postsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get the total count for pagination
            var totalCount = await postsQuery.CountAsync();
            vm.Posts = new StaticPagedList<Post>(posts, pageNumber, pageSize, totalCount);

            // Return the view with the data
            return View(vm); // This should map to /Areas/Admin/Views/Home/Index.cshtml
        }
    }
}
