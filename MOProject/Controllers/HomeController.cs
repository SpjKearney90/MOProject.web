using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using System.Diagnostics;
using X.PagedList;

namespace MOProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
                                ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var vm = new HomeVM();

            // Fetch settings asynchronously
            var setting = await _context.Settings!.FirstOrDefaultAsync(); // Asynchronously get first setting
            if (setting != null)
            {
                vm.Title = setting.Title;
                vm.ShortDescription = setting.ShortDescription;
                vm.ThumbnailUrl = setting.ThumbnailUrl;
            }

            // Pagination setup
            int pageSize = 4; // Number of posts per page
            int pageNumber = page ?? 1; // Default to page 1 if not provided

            // Query posts and apply pagination using Skip and Take
            var postsQuery = _context.Posts!
                .Include(x => x.ApplicationUser) // Include related data (e.g., ApplicationUser)
                .OrderByDescending(x => x.CreatedDate); // Order by creation date (descending)

            // Get the posts for the current page asynchronously
            var posts = await postsQuery
                .Skip((pageNumber - 1) * pageSize)  // Skip the records for previous pages
                .Take(pageSize)                     // Take the required number of posts for this page
                .ToListAsync();                     // Execute the query asynchronously and fetch the results

            // Create a StaticPagedList to support paging in the view
            var totalCount = await postsQuery.CountAsync(); // Get the total count of posts
            vm.Posts = new StaticPagedList<Post>(posts, pageNumber, pageSize, totalCount);

            // Return the view with the model containing the posts and other data
            return View("Views/Blog", vm); // Use the correct path or adjust accordingly

            



        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
