using AspNetCoreHero.ToastNotification.Abstractions;
using MOProject.Data;  // Updated namespace for ApplicationDbContext
using MOProject.Models;  // Updated namespace for Setting and ApplicationUser
using MOProject.ViewModels; // Assuming ViewModel is in the same namespace or relevant namespace
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace MOProject.Areas.Admin.Controllers // Adjusted to match your project namespace
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor to initialize dependencies
        public SettingController(ApplicationDbContext context,
                              
                                 IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
          
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetch the settings from the database
            var settings = await _context.Settings!.ToListAsync();
            if (settings.Count > 0)
            {
                var vm = new SettingsVM()
                {
                    Id1 = settings[0].Id1,
                    SiteName1 = settings[0].SiteName1,
                    Title1 = settings[0].Title1,
                    ShortDescription1 = settings[0].ShortDescription1,
                    ThumbnailUrl1 = settings[0].ThumbnailUrl1,
                    FacebookUrl1 = settings[0].FacebookUrl1,
                    InstagramUrl1 = settings[0].InstagramUrl1,

                };
                return View(vm);
            }

            // If no settings are found, create a default setting
            var setting = new Setting1()
            {
                SiteName = "Demo Name",
            };
            await _context.Settings!.AddAsync(setting);
            await _context.SaveChangesAsync();

            var createdSettings = await _context.Settings!.ToListAsync();
            var createdVm = new SettingsVM()
            {
                Id1 = createdSettings[0].Id1,
                SiteName1 = createdSettings[0].SiteName1,
                Title1 = createdSettings[0].Title1,
                ShortDescription1 = createdSettings[0].ShortDescription1,
                ThumbnailUrl1 = createdSettings[0].ThumbnailUrl1,
                FacebookUrl1 = createdSettings[0].FacebookUrl1,
                InstagramUrl1 = createdSettings[0].InstagramUrl1,

            };
            return View(createdVm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingsVM vm)
        {
            // Validate the model
            if (!ModelState.IsValid) { return View(vm); }

            // Retrieve the setting to update
            var setting = await _context.Settings!.FirstOrDefaultAsync(x => x.Id1 == vm.Id1);
            if (setting == null)
            {
                
                return View(vm);
            }

            // Update setting properties
            setting.SiteName1 = vm.SiteName1;
            setting.Title1 = vm.Title1;
            setting.ShortDescription1 = vm.ShortDescription1;
            setting.FacebookUrl1 = vm.FacebookUrl1;


            //if (vm.Thumbnail != null)
            //{
            //    setting.ThumbnailUrl = UploadImage(vm.Thumbnail);
            //}

            // Save changes to the database
            await _context.SaveChangesAsync();
           
            return RedirectToAction("Index", "Setting", new { area = "Admin" });
        }

        // Helper method to upload image and generate unique file name
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
