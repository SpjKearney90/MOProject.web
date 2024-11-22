using AspNetCoreHero.ToastNotification.Abstractions;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetch or create settings
            var setting = await _context.Settings!.FirstOrDefaultAsync();
            if (setting == null)
            {
                setting = new Setting1
                {
                    SiteName1 = "Demo Name"
                };

                await _context.Settings.AddAsync(setting);
                await _context.SaveChangesAsync();
            }

            var vm = MapToViewModel(setting);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Settings1VM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Find the setting and update its properties
            var setting = await _context.Settings!.FirstOrDefaultAsync(x => x.Id1 == vm.Id1);
            if (setting == null) return View(vm);

            UpdateSetting(setting, vm);

            if (vm.Thumbnail1 != null)
            {
                setting.ThumbnailUrl1 = UploadImage(vm.Thumbnail1);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UploadImage(IFormFile file)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            Directory.CreateDirectory(folderPath); // Ensure the directory exists

            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        private Settings1VM MapToViewModel(Setting1 setting)
        {
            return new Settings1VM
            {
                Id1 = setting.Id1,
                SiteName1 = setting.SiteName1,
                Title1 = setting.Title1,
                ShortDescription1 = setting.ShortDescription1,
                ThumbnailUrl1 = setting.ThumbnailUrl1,
                FacebookUrl1 = setting.FacebookUrl1,
                InstagramUrl1 = setting.InstagramUrl1
            };
        }

        private void UpdateSetting(Setting1 setting, Settings1VM vm)
        {
            setting.SiteName1 = vm.SiteName1;
            setting.Title1 = vm.Title1;
            setting.ShortDescription1 = vm.ShortDescription1;
            setting.FacebookUrl1 = vm.FacebookUrl1;
            setting.InstagramUrl1 = vm.InstagramUrl1;
        }
    }
}
