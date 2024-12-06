using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Extensions;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Blog page action with pagination
        public IActionResult Index(int? page)
        {
            var vm = new HomeVM();

            // Handle Settings
            var settings = _context.Settings!.ToList();
            if (settings == null || !settings.Any())
            {
                vm.Title = "Default Title";
                vm.ShortDescription = "Default Description";
                vm.ThumbnailUrl = "Thumbnail.png"; // Default thumbnail name
            }
            else
            {
                vm.Title = settings[0].Title;
                vm.ShortDescription = settings[0].ShortDescription;
                vm.ThumbnailUrl = settings[0].ThumbnailUrl;
            }

            // Handle Posts
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            vm.Posts = _context.Posts?
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.CreatedDate)
                .ToPagedList(pageNumber, pageSize) ?? new PagedList<Post>(new List<Post>(), pageNumber, pageSize);

            return View(vm);
        }

        // Privacy action
        public IActionResult Privacy()
        {
            return View();
        }

        // Error action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
