using System.Security.Claims;
using System.Threading.Tasks;
using FS0924_S18_L4.Models;
using FS0924_S18_L4.Services;
using FS0924_S18_L4.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FS0924_S18_L4.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SchoolClassService _schoolClassService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            SchoolClassService schoolClassService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _schoolClassService = schoolClassService;
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
                new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
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

        [Authorize(Roles = "Director")]
        public async Task<IActionResult> Index()
        {
            var users = new UsersListViewModel();

            users.Users = await _userManager
                .Users.Include(u => u.ApplicationUserRole)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.SchoolClass)
                .ToListAsync();

            return View(users);
        }

        [Authorize(Roles = "Director")]
        [HttpGet("Account/EditbyAdmin/{id:guid}")]
        public async Task<IActionResult> EditbyAdmin(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Index");
            }

            var editUser = new EditUserByAdminViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                SchoolClassId = user.SchoolClassId,
            };

            var classes = await _schoolClassService.GetAllSchoolClasses();

            ViewBag.Classes = classes;

            return View(editUser);
        }

        [Authorize(Roles = "Director")]
        public async Task<IActionResult> EditbyAdmin(EditUserByAdminViewModel editUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data";
                return View(editUser);
            }

            var user = await _userManager.FindByIdAsync(editUser.Id.ToString());

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return View(editUser);
            }

            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.SchoolClassId = editUser.SchoolClassId;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }
    }
}
