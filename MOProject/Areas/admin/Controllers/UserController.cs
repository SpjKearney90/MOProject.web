using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Models;
using MOProject.ViewModels;
using Microsoft.Extensions.Logging;

namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            _logger.LogInformation("Login page accessed.");
            return View(new LoginVM());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation error.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
                return View(vm);
            }

            _logger.LogInformation("Received Username: {Username}", vm.Username);
            _logger.LogInformation("Received Password: {Password}", vm.Password);

            var checkUsername = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);
            if (checkUsername == null)
            {
                _logger.LogWarning("User not found.");
                return View(vm);
            }

            var verifyPassword = await _userManager.CheckPasswordAsync(checkUsername, vm.Password!);
            if (!verifyPassword)
            {
                _logger.LogWarning("Incorrect password.");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Username!, vm.Password!, vm.RememberMe, true);
            _logger.LogInformation("User {Username} logged in successfully.", vm.Username);
            _logger.LogInformation("Redirecting to Dash...");

            return RedirectToAction("Dash", "User", new { area = "Admin" });
        }

        public IActionResult Dash()
        {
            _logger.LogInformation("Dash page accessed.");
            return View();
        }
    }
}
