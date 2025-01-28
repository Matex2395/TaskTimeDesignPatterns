using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;
using TaskTimePredicter.ViewModels;
using TaskTimeDesignPatterns.Interfaces;

namespace TaskTimePredicter.Controllers
{
    public class AccessController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUserFactory _userFactory;

        public AccessController(AppDbContext context, IUserFactory userFactory)
        {
            _db = context;
            _userFactory = userFactory;
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

            try
            {
                // Usar UserFactory para crear un nuevo usuario
                User user = _userFactory.CreateUser(
                    userName: model.Name,
                    userEmail: model.Email,
                    userPassword: model.Password,
                    userRole: "Developer" // Asignar rol de "Desarrollador" por defecto
                );

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                if (user.UserId != 0)
                {
                    return RedirectToAction("Login", "Access");
                }
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ha ocurrido un error inesperado al registrar el usuario.";
            }

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
