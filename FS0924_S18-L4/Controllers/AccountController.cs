using FS0924_S18_L4.Models;
using FS0924_S18_L4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FS0924_S18_L4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data";
                return View(registerUser);
            }

            var newUser = new ApplicationUser()
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                BirthDate = registerUser.BirthDate,
                UserName = registerUser.Email,
                Email = registerUser.Email,
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Error creating user";
                return View(registerUser);
            }

            var user = await _userManager.FindByEmailAsync(registerUser.Email);

            if (user == null)
            {
                TempData["Error"] = "Error creating user";
                return View(registerUser);
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return RedirectToAction("Index", "Home");
        }
    }
}
