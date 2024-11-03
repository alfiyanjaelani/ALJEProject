using ALJEproject.Data;
using ALJEproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALJEproject.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private readonly ALJEprojectDbContext _context;

        // Constructor that combines both dependencies
        public MenuController(ALJEprojectDbContext context, ILogger<MenuController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.ToListAsync();
            return View(menus);
        }

        // GET: Menu/Create
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateMenuPartial");
        }

        // POST: Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                menu.CreatedDate = DateTime.Now;
                menu.CreatedBy = "System"; // Set as appropriate, e.g., logged-in user
                _context.Menus.Add(menu);
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList() });
        }

        // GET: Menu/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var menu = _context.Menus.Find(id);
            if (menu == null)
            {
                return NotFound();
            }

            return PartialView("_EditMenuPartial", menu);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    menu.UpdatedDate = DateTime.Now;
                    menu.UpdatedBy = "System"; // Set as appropriate, e.g., logged-in user
                    _context.Update(menu);
                    _context.SaveChanges();
                    _logger.LogInformation("Menu with ID {MenuId} updated successfully.", menu.MenuID);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating menu with ID {MenuId}.", menu.MenuID);
                    return Json(new { success = false, message = "An error occurred while updating the menu." });
                }
            }

            var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            _logger.LogError("Failed to update menu with ID {MenuId}. Model state is invalid.", menu.MenuID);

            return Json(new { success = false, errors = errorMessages });
        }

        // POST: Menu/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var menu = _context.Menus.Find(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                _context.SaveChanges();
                _logger.LogInformation("Menu with ID {MenuId} deleted successfully.", id);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = new[] { "Menu not found." } });
        }
    }
}
