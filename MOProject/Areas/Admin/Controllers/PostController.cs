using Microsoft.AspNetCore.Mvc;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        public IActionResult BlogPost()
        {
            return View();
        }
    }
}
