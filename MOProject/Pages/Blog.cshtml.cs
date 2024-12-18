using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MOProject.Pages
{
    public class BlogModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BlogModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // This is the ViewModel you will use in your Razor Page
        public HomeVM HomeVM { get; set; }

        // OnGet method will handle the logic to fetch data for the page
        public async Task OnGetAsync(int? page)
        {
            HomeVM = new HomeVM();

            // Fetch the setting data for the page (Title, ShortDescription, etc.)
            var setting = await _context.Settings!.FirstOrDefaultAsync();
            if (setting != null)
            {
                HomeVM.Title = setting.Title;
                HomeVM.ShortDescription = setting.ShortDescription;
                HomeVM.ThumbnailUrl = setting.ThumbnailUrl;
            }

            // Pagination setup
            int pageSize = 5;  // Number of posts per page
            int pageNumber = page ?? 1;  // Default to page 1 if not provided

            // Query posts and apply pagination
            var postsQuery = _context.Posts!
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.CreatedDate);

            // Fetch posts for the current page
            var posts = await postsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get the total count for pagination
            var totalCount = await postsQuery.CountAsync();
            HomeVM.Posts = new StaticPagedList<Post>(posts, pageNumber, pageSize, totalCount);

            // Calculate total pages and add to HomeVM
            HomeVM.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            HomeVM.CurrentPage = pageNumber;
        }
    }
}
