using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _context.SiteSettings!.ToListAsync();

            if (settings.Count > 0)
            {
                var vm = new SettingVM
                {
                    Id = settings[0].Id,
                    SiteName = settings[0].SiteName,
                    Title = settings[0].Title,
                    ShortDescription = settings[0].ShortDescription,
                    ThumbnailUrl = settings[0].ThumbnailUrl,
                    FacebookUrl = settings[0].FacebookUrl,
                    InstagramUrl = settings[0].InstagramUrl,
                };

                return View(vm);
            }
            else
            {
                var setting = new SiteSetting()
                {
                    SiteName = "Demo Name",
                };

                // Add the new setting to the context and save changes
                await _context.SiteSettings!.AddAsync(setting);
                await _context.SaveChangesAsync();

                var createdSettings = await _context.SiteSettings!.ToListAsync();
                var createdVm = new SettingVM
                {
                    Id = createdSettings[0].Id,
                    SiteName = createdSettings[0].SiteName,
                    Title = createdSettings[0].Title,
                    ShortDescription = createdSettings[0].ShortDescription,
                    ThumbnailUrl = createdSettings[0].ThumbnailUrl,
                    FacebookUrl = createdSettings[0].FacebookUrl,
                    InstagramUrl = createdSettings[0].InstagramUrl,
                };

                return View(createdVm);
            }
        }
    }
}
