using Microsoft.AspNetCore.Mvc;

namespace MOProject.Areas.admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }
    }
}
