using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Setting
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetch the first setting from the database or create a default one
            var setting = await _context.Settings!.FirstOrDefaultAsync();
            if (setting == null)
            {
                setting = new Setting
                {
                    SiteName = "Demo Name"
                };
                await _context.Settings!.AddAsync(setting);
                await _context.SaveChangesAsync();
            }

            // Map to the ViewModel
            var vm = new SettingVM
            {
                Id = setting.Id,
                SiteName = setting.SiteName,
                Title = setting.Title,
                ShortDescription = setting.ShortDescription,
                FacebookUrl = setting.FacebookUrl,
                InstagramUrl = setting.InstagramUrl,
                ThumbnailUrl = setting.ThumbnailUrl
            };

            return View(vm);
        }

        // POST: Admin/Setting
        [HttpPost]
        public async Task<IActionResult> Index(SettingVM vm)
        {
            // Validate the form data
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Fetch the existing setting record
            var setting = await _context.Settings!.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (setting == null)
            {
                ModelState.AddModelError("", "Settings not found.");
                return View(vm);
            }

            // Update the setting properties
            setting.SiteName = vm.SiteName;
            setting.Title = vm.Title;
            setting.ShortDescription = vm.ShortDescription;
            setting.FacebookUrl = vm.FacebookUrl;
            setting.InstagramUrl = vm.InstagramUrl;

            // Handle the thumbnail upload
            if (vm.Thumbnail != null)
            {
                setting.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.SaveChangesAsync();
            return Redirect("Blog");
        }

        // Helper method to upload an image
        private string UploadImage(IFormFile file)
        {
            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Ensure the folder exists
            Directory.CreateDirectory(folderPath);

            // Save the file to the server
            using (var fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
