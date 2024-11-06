using Microsoft.AspNetCore.Mvc;
using MOProject.ViewModels;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        public IActionResult BlogPost()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }



    }
}
