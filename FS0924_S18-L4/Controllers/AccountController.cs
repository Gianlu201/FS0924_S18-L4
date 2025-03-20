using System.Security.Claims;
using FS0924_S18_L4.Models;
using FS0924_S18_L4.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data";
                return View(loginUser);
            }

            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user == null)
            {
                TempData["Error"] = "Invalid email or password";
                return View(loginUser);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                loginUser.Password,
                true,
                false
            );

            if (!signInResult.Succeeded)
            {
                TempData["Error"] = "Invalid email or password";
                return View(loginUser);
            }

            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
