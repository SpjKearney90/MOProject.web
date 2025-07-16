using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.IO;
using MOProject.Models;
using MOProject.ViewModels;
using System.Collections.Generic;

namespace MOProject.Pages
{
    public class BlogModel : PageModel
    {
        public BlogPageViewModel ViewModel { get; set; } = new();

        public void OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Blog", "blogData.json");

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                ViewModel.Posts = JsonSerializer.Deserialize<List<BlogPost>>(json);
            }
            else
            {
                ViewModel.Posts = new List<BlogPost>();
            }
        }
    }
}
