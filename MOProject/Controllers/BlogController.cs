using Microsoft.AspNetCore.Mvc;
using MOProject.Data;
using System.Runtime.CompilerServices;

namespace MOProject.Controllers
{
    public class BlogController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)

        {

            _context = context;
        }


        public IActionResult Post()
        {
            return View();
        }
    }
}
