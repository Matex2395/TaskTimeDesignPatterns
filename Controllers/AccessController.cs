using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;
using TaskTimePredicter.ViewModels;

namespace TaskTimePredicter.Controllers
{
    public class AccessController : Controller
    {
        private readonly AppDbContext _db;

        public AccessController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Restricted()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden";
                return View();
            }

            User user = new User
            {
                UserName = model.Name,
                UserEmail = model.Email,
                UserPassword = model.Password,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                UserRole = "Developer"
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            if (user.UserId != 0) return RedirectToAction("Login", "Access");

            TempData["ErrorMessage"] = "Ha ocurrido un error al registrar el usuario.";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User? found_user = await _db.Users.FirstOrDefaultAsync(u => u.UserEmail == model.Email && u.UserPassword == model.Password);
            if (found_user == null)
            {
                TempData["ErrorMessage"] = "No se encontraron coincidencias.";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, found_user.UserName),
                new Claim(ClaimTypes.Role, found_user.UserRole),
                new Claim(ClaimTypes.NameIdentifier, found_user.UserId.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);
            return RedirectToAction("Index", "Home");

        }
    }
}
