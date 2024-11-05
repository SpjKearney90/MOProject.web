using Microsoft.AspNetCore.Mvc;

namespace MOProject.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
