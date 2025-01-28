using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskTimePredicter.Data;
using TaskTimePredicter.Models;

namespace TaskTimePredicter.Controllers
{
    [Authorize]
    public class QuestsController : Controller
    {
        private readonly AppDbContext _context;

        public QuestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Quests
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Quests.Include(q => q.Category).Include(q => q.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Quests/Complete/5
        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }
            return View(quest);
        }

        // POST: Quests/Complete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, [Bind("QuestId,QuestName,EstimatedTime,ActualTime,QuestState,CreationDate,UserId,SubcategoryId,ProjectId")] Quest quest)
        {
            if (id != quest.QuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Mantenimiento CreationDate
                    var prevQuest = await _context.Quests.AsNoTracking().FirstOrDefaultAsync(d => d.QuestId == quest.QuestId);
                    if (prevQuest == null)
                    {
                        return NotFound();
                    }
                    quest.CreationDate = prevQuest.CreationDate;
                    //Búsqueda de Usuario correspondiente por UserId
                    if (quest.UserId.HasValue)
                    {
                        quest.User = await _context.Users
                            .FirstOrDefaultAsync(u => u.UserId == quest.UserId.Value);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Usuario no encontrado o no tiene Id asignado";
                        return View(quest);
                    }
                    //Búsqueda de Subcategoría correspondiente por SubcategoryId
                    if (quest.SubcategoryId.HasValue)
                    {
                        quest.Subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                        var subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                        if (subcategory != null)
                        {
                            quest.CategoryId = subcategory.CategoryId;
                            quest.Category = await _context.Categories
                                .FirstOrDefaultAsync(c => c.CategoryId == quest.CategoryId);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Subcategoría no válida.";
                            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                            return View(quest);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Subcategoría no encontrada o no tiene Id asignado";
                        return View(quest);
                    }
                    //Búsqueda de Proyecto correspondiente por ProyectId
                    if (quest.ProjectId.HasValue)
                    {
                        quest.Project = await _context.Projects
                            .FirstOrDefaultAsync(p => p.ProjectId == quest.ProjectId.Value);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Subcategoría no encontrada o no tiene Id asignado";
                        return View(quest);
                    }
                    _context.Update(quest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestExists(quest.QuestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(quest);
        }


        // GET: Quests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Subcategory)
                .Include(q => q.Project)
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // GET: Quests/Create
        public IActionResult Create()
        {
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: Quests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestId,QuestName,EstimatedTime,ActualTime,QuestState,CreationDate,UserId,SubcategoryId,ProjectId")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                //Validación 'CreatedAt' != Nulo ni vacío
                if (quest.CreationDate == default)
                {
                    quest.CreationDate = DateOnly.FromDateTime(DateTime.Now);
                }

                //Búsqueda de Usuario correspondiente por UserId
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim != null)
                {
                    quest.UserId = int.Parse(userIdClaim);
                    quest.User = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserId == quest.UserId.Value);
                }
                //Búsqueda de Subcategoría correspondiente por SubcategoryId
                if (quest.SubcategoryId.HasValue)
                {
                    quest.Subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                    var subcategory = await _context.Subcategories
                        .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                    if (subcategory != null)
                    {
                        quest.CategoryId = subcategory.CategoryId;
                        quest.Category = await _context.Categories
                            .FirstOrDefaultAsync(c => c.CategoryId == quest.CategoryId);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Subcategoría no válida.";
                        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                        ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                        return View(quest);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Seleccione una Subcategoría para Asociar";
                    ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                    ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                    return View(quest);
                }
                if (quest.ProjectId.HasValue)
                {
                    quest.Project = await _context.Projects
                        .FirstOrDefaultAsync(p => p.ProjectId == quest.ProjectId.Value);
                }
                else
                {
                    TempData["ErrorMessage"] = "Seleccione un Proyecto para Asociar";
                    ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                    ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                    return View(quest);
                }

                _context.Add(quest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
            return View(quest);
        }

        // GET: Quests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests.FindAsync(id);
            if (quest == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
            return View(quest);
        }

        // POST: Quests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestId,QuestName,EstimatedTime,ActualTime,QuestState,CreationDate,UserId,SubcategoryId,ProjectId")] Quest quest)
        {
            if (id != quest.QuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prevQuest = await _context.Quests.FirstOrDefaultAsync(d => d.QuestId == quest.QuestId);
                    quest.CreationDate = prevQuest.CreationDate;
                    //Búsqueda de Usuario correspondiente por UserId
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userIdClaim != null)
                    {
                        quest.UserId = int.Parse(userIdClaim);
                        quest.User = await _context.Users
                            .FirstOrDefaultAsync(u => u.UserId == quest.UserId.Value);
                    }
                    //Búsqueda de Subcategoría correspondiente por SubcategoryId
                    if (quest.SubcategoryId.HasValue)
                    {
                        quest.Subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                        var subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(s => s.SubcategoryId == quest.SubcategoryId.Value);
                        if (subcategory != null)
                        {
                            quest.CategoryId = subcategory.CategoryId;
                            quest.Category = await _context.Categories
                                .FirstOrDefaultAsync(c => c.CategoryId == quest.CategoryId);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Subcategoría no válida.";
                            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                            return View(quest);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Seleccione una Subcategoría para Asociar";
                        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                        ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                        return View(quest);
                    }
                    if (quest.ProjectId.HasValue)
                    {
                        quest.Project = await _context.Projects
                            .FirstOrDefaultAsync(p => p.ProjectId == quest.ProjectId.Value);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Seleccione un Proyecto para Asociar";
                        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
                        ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
                        return View(quest);
                    }
                    _context.Update(quest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestExists(quest.QuestId))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategories, "SubcategoryId", "SubcategoryName");
            return View(quest);
        }

        // GET: Quests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quest = await _context.Quests
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Subcategory)
                .Include(q => q.Project)
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (quest == null)
            {
                return NotFound();
            }

            return View(quest);
        }

        // POST: Quests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quest = await _context.Quests.FindAsync(id);
            if (quest != null)
            {
                _context.Quests.Remove(quest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestExists(int id)
        {
            return _context.Quests.Any(e => e.QuestId == id);
        }

        // CORE
        public IActionResult Analyze()
        {
            var proyectos = _context.Projects.ToList();
            var categorias = _context.Categories.ToList();

            var resultadosProyectos = new List<object>();
            var resultadosCategorias = new List<object>();

            // Análisis de Productividad por Proyectos
            foreach (var proyecto in proyectos)
            {
                var tareasProyecto = _context.Quests.Where(q => q.ProjectId == proyecto.ProjectId).ToList();
                double totalTiempoEstimado = 0;
                double totalTiempoReal = 0;

                foreach (var tarea in tareasProyecto)
                {
                    totalTiempoEstimado += tarea.EstimatedTime;
                    totalTiempoReal += tarea.ActualTime ?? 0;
                }

                if (totalTiempoEstimado > 0)
                {
                    double eficiencia = totalTiempoEstimado / totalTiempoReal * 100;
                    resultadosProyectos.Add(new
                    {
                        Proyecto = proyecto.ProjectName,
                        TiempoEstimadoTotal = totalTiempoEstimado,
                        TiempoRealTotal = totalTiempoReal,
                        Eficiencia = eficiencia
                    });
                }
            }

            // Análisis de Tiempo Promedio por Categoría y Subcategoría
            var categoriasTareas = _context.Categories
                .Select(c => new
                {
                    CategoriaId = c.CategoryId,
                    CategoriaName = c.CategoryName,
                    Subcategorias = _context.Subcategories
                .Where(s => s.CategoryId == c.CategoryId)
                .Select(s => new
                {
                    SubcategoryId = s.SubcategoryId,
                    SubcategoryName = s.SubcategoryName
                })
                .ToList()
                }).ToList();

            foreach (var categoria in categoriasTareas)
            {
                if (!categoria.Subcategorias.Any())
                {
                    resultadosCategorias.Add(new
                    {
                        Categoria = categoria.CategoriaName,
                        Subcategoria = "N/A",
                        TiempoPromedioEstimado = 0,
                        TiempoPromedioReal = 0
                    });
                }
                else
                {
                    foreach (var subcategoria in categoria.Subcategorias)
                    {
                        var tareasSubcategoria = _context.Quests
                            .Where(q => q.SubcategoryId == subcategoria.SubcategoryId)
                            .ToList();

                        double totalTiempoReal = 0;
                        double totalTiempoEstimado = 0;
                        int count = 0;

                        foreach (var tarea in tareasSubcategoria)
                        {
                            totalTiempoEstimado += tarea.EstimatedTime;
                            if (tarea.ActualTime.HasValue)
                            {
                                totalTiempoReal += tarea.ActualTime.Value;
                                count++;
                            }
                        }

                        if (count > 0)
                        {
                            double tiempoPromedioReal = totalTiempoReal / count;
                            double tiempoPromedioEstimado = totalTiempoEstimado / count;
                            resultadosCategorias.Add(new
                            {
                                Categoria = categoria.CategoriaName,
                                Subcategoria = subcategoria.SubcategoryName,
                                TiempoPromedioEstimado = tiempoPromedioEstimado,
                                TiempoPromedioReal = tiempoPromedioReal
                            });
                        }
                        else
                        {
                            resultadosCategorias.Add(new
                            {
                                Categoria = categoria.CategoriaName,
                                Subcategoria = subcategoria.SubcategoryName,
                                TiempoPromedioEstimado = 0,
                                TiempoPromedioReal = 0
                            });
                        }
                    }
                }
            }

            var resultadosCombinados = new
            {
                ProductividadProyectos = resultadosProyectos,
                TiempoPromedioCategorias = resultadosCategorias
            };

            return View(resultadosCombinados);
        }

        // DEFENSA DEL CORE
        public async Task<IActionResult> Statistics()
        {
            var projectTimesQuery = await _context.Quests
        .Where(q => q.ActualTime.HasValue && q.ProjectId != null)
        .GroupBy(q => q.ProjectId)
        .Select(g => new
        {
            ProjectId = g.Key,
            TotalTime = g.Sum(q => q.ActualTime.Value)
        })
        .ToListAsync();

            var projectTimes = projectTimesQuery.ToDictionary(pt => pt.ProjectId.Value, pt => pt.TotalTime);

            if (projectTimes.Count == 0)
            {
                return View();
            }

            //Ordenar por método OrderBy
            //var maxTimeProject = projectTimes.OrderByDescending(p => p.Value).First();
            //var minTimeProject = projectTimes.OrderBy(p => p.Value).First();

            var projectTimesList = projectTimes.ToList();
            KeyValuePair<int, double> maxTimeProject = projectTimesList[0];
            KeyValuePair<int, double> minTimeProject = projectTimesList[0];

            foreach (var projectTime in projectTimesList)
            {
                if (projectTime.Value > maxTimeProject.Value)
                {
                    maxTimeProject = projectTime;
                }
                else if (projectTime.Value < minTimeProject.Value)
                {
                    minTimeProject = projectTime;
                }
            }

            var timeDifference = maxTimeProject.Value - minTimeProject.Value;
            var Estadisticas = new
            {
                ProyectoMax = maxTimeProject.Key == minTimeProject.Key ? "N/A" : _context.Projects.Find(maxTimeProject.Key)?.ProjectName,
                MaxTime = maxTimeProject.Value,
                ProyectoMin = _context.Projects.Find(minTimeProject.Key)?.ProjectName,
                MinTime = minTimeProject.Value,
                DiffMinMax = timeDifference
            };
            return View(Estadisticas);
        }
    }
}
