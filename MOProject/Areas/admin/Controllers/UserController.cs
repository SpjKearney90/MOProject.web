
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MOProject.Models;
using MOProject.ViewModels;


namespace MOProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public IActionResult Index()
        {
            return View();

        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            
            {
                return View(new LoginVM());
            }
            
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var checkUsername = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.Username);
            if (checkUsername == null)
            {
                return View(vm);
            }
            var verifyPassword = await _userManager.CheckPasswordAsync(checkUsername, vm.Password);
            if (!verifyPassword)
            {
                return View(vm);
            }
            await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, true);

            
            

   
            return RedirectToAction("Index", "User", new { area = "Admin" });

        }



    }
}
