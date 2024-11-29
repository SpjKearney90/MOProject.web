using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Extensions;

namespace FineBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            var vm = new HomeVM();
            var settings = _context.Settings!.ToList();
            vm.Title = settings[0].Title;
            vm.ShortDescription = settings[0].ShortDescription;
            vm.ThumbnailUrl = settings[0].ThumbnailUrl;

            int pageSize = 4;
            int pageNumber = page ?? 1;

            vm.Posts = _context.Posts!
                               .Include(x => x.ApplicationUser)
                               .OrderByDescending(x => x.CreatedDate)
                               .ToPagedList(pageNumber, pageSize);

            return View(vm);
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
