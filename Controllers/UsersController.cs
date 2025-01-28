using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimePredicter.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserFactory _userFactory;

        public UsersController(AppDbContext context, IUserFactory userFactory)
        {
            _context = context;
            _userFactory = userFactory;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Developer")) return RedirectToAction("Restricted", "Access");
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var userRoles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Desarrollador", Value = "Developer"},
                new SelectListItem { Text = "Administrador", Value = "Administrator"},
            };
            ViewData["UserRole"] = userRoles;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,UserEmail,UserPassword,UserRole,CreatedAt")] User userInput)
        {
            var userRoles = new List<SelectListItem>
                        {
                            new SelectListItem { Text = "Desarrollador", Value = "Developer"},
                            new SelectListItem { Text = "Administrador", Value = "Administrator"},
                        };
            if (ModelState.IsValid)
            {
                try
                {
                    // Usar UserFactory para crear un nuevo usuario
                    var newUser = _userFactory.CreateUser(
                        userInput.UserName,
                        userInput.UserEmail,
                        userInput.UserPassword,
                        userInput.UserRole
                    );

                    _context.Add(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            ViewData["UserRole"] = userRoles;
            return View(userInput);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Desarrollador", Value = "Developer"},
                new SelectListItem { Text = "Administrador", Value = "Administrator"},
            };
            ViewData["UserRole"] = userRoles;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,UserPassword,UserRole,CreatedAt")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prevUser = await _context.Users.FirstOrDefaultAsync(d => d.UserId == user.UserId);
                    user.CreatedAt = prevUser.CreatedAt;
                    //Validación 'CreatedAt' != Nulo ni vacío
                    if (user.CreatedAt == default)
                    {
                        user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var userRoles = new List<SelectListItem>
            {
                new SelectListItem { Text = "Desarrollador", Value = "Developer"},
                new SelectListItem { Text = "Administrador", Value = "Administrator"},
            };
            ViewData["UserRole"] = userRoles;
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
