using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.IO;
using MOProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MOProject.Pages
{
    public class PostModel : PageModel
    {
        public BlogPost Post { get; set; }

        public void OnGet(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                Post = null;
                return;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Blog", "blogData.json");

            if (!System.IO.File.Exists(filePath))
            {
                Post = null;
                return;
            }

            var json = System.IO.File.ReadAllText(filePath);
            var posts = JsonSerializer.Deserialize<List<BlogPost>>(json);

            // Find the post with matching slug (case-insensitive)
            Post = posts?.FirstOrDefault(p => p.Slug?.Equals(slug, System.StringComparison.OrdinalIgnoreCase) == true);
        }
    }
}
