using Microsoft.AspNetCore.Mvc;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using NuGet.Protocol;
using System.Drawing.Text;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        public PostController(ApplicationDbContext context , 
                    IWebHostEnvironment webHostEnvironment) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult BlogPost()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatePostVM());
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreatePostVM vm)
        {
            
                if (ModelState.IsValid) { return View(vm); }

            var post = new Post();

            post.Title = vm.Title!.Trim();
            post.Description = vm.Description;
            post.ShortDescription= vm.ShortDescription;

            if (post.Title != null)
            {
             
                    string slug = vm.Title.Trim();
                    slug = slug.Replace(" ", "-");
                    post.Slug = slug + "-" + Guid.NewGuid();
                
            }


            if(vm.Thumbnail != null)
            {
               post.ThumbnailUrl= UploadImage(vm.Thumbnail);
            }

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("dash");


                return RedirectToAction("dash");
            }

            private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var foldePath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName ;
            var filePath = Path.Combine(foldePath, uniqueFileName);
            using(FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

        }
    }

