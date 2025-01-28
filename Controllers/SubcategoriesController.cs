using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimePredicter.Controllers
{
    public class SubcategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public SubcategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Subcategories
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Developer")) return RedirectToAction("Restricted", "Access");
            var appDbContext = _context.Subcategories.Include(s => s.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Subcategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _context.Subcategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubcategoryId == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return View(subcategory);
        }

        // GET: Subcategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Subcategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcategoryId,SubcategoryName,SubcategoryDescription,CategoryId")] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                //Búsqueda de Categoría correspondiente por CategoryId
                if (subcategory.CategoryId.HasValue)
                {
                    subcategory.Category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.CategoryId == subcategory.CategoryId.Value);
                    _context.Add(subcategory);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Seleccione una Categoría para Asociar";
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subcategory.CategoryId);
            return View(subcategory);
        }

        // GET: Subcategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _context.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subcategory.CategoryId);
            return View(subcategory);
        }

        // POST: Subcategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcategoryId,SubcategoryName,SubcategoryDescription,CategoryId")] Subcategory subcategory)
        {
            if (id != subcategory.SubcategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (subcategory.CategoryId.HasValue)
                    {
                        subcategory.Category = await _context.Categories
                            .FirstOrDefaultAsync(c => c.CategoryId == subcategory.CategoryId.Value);
                    }
                    _context.Update(subcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcategoryExists(subcategory.SubcategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", subcategory.CategoryId);
            return View(subcategory);
        }

        // GET: Subcategories/Delete/5
        // PROHIBIDO PARA ADMIN Y DEVELOPER
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var subcategory = await _context.Subcategories
        //        .Include(s => s.Category)
        //        .FirstOrDefaultAsync(m => m.SubcategoryId == id);
        //    if (subcategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(subcategory);
        //}

        // POST: Subcategories/Delete/5
        // PROHIBIDO PARA ADMIN Y DEVELOPER
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var subcategory = await _context.Subcategories.FindAsync(id);
        //    if (subcategory != null)
        //    {
        //        _context.Subcategories.Remove(subcategory);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SubcategoryExists(int id)
        {
            return _context.Subcategories.Any(e => e.SubcategoryId == id);
        }
    }
}
