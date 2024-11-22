using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using X.PagedList;
using MOProject.Data;
using MOProject.Models;
using MOProject.ViewModels;
using Microsoft.Extensions.Logging;
using X.PagedList.Extensions;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ApplicationDbContext context,
                              ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        // Dashboard with paginated posts
        public async Task<IActionResult> Dash(int? page)
        {
            var loggedInUser = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            if (loggedInUser == null)
            {
                _logger.LogWarning("User not found during dashboard access.");
                return RedirectToAction("Login", "User", new { area = "Admin" });
            }

            // Fetch all posts since you are the sole administrator
            var listOfPosts = await _context.Posts
                .Include(x => x.ApplicationUser)
                .ToListAsync();

            // Map to ViewModel
            var postsVM = listOfPosts.Select(x => new PostVM
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName = $"{x.ApplicationUser.FirstName} {x.ApplicationUser.LastName}"
            }).ToList();

            int pageSize = 5; // Items per page
            int pageNumber = page ?? 1; // Default to page 1 if null

            return View(postsVM.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, pageSize));
        }

        // Login page (GET)
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        // Login page (POST)
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var existingUser = await _userManager.FindByEmailAsync(vm.Username);

            if (existingUser == null)
            {
                _logger.LogWarning("Invalid login attempt for email {Email}.", vm.Username);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(existingUser, vm.Password, vm.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserName} logged in successfully.", existingUser.UserName);
                return RedirectToAction("Dash"); // Redirect to dashboard
            }

            _logger.LogWarning("Invalid password for user {UserName}.", existingUser.UserName);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(vm);
        }
    }
}
