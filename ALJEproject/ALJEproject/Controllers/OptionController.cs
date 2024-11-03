using ALJEproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using ALJEproject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ALJEproject.Controllers
{
    public class OptionController : Controller
    {
        private readonly ILogger<OptionController> _logger;
        private readonly ALJEprojectDbContext _context;

        // Constructor for dependency injection
        public OptionController(ALJEprojectDbContext context, ILogger<OptionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Display the list of roles (or options)
        public async Task<IActionResult> Index()
        {
            var Options = await _context.Options.ToListAsync();
            return View(Options);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateOptionPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Option option)
        {
            if (ModelState.IsValid)
            {
                option.CreatedDate = DateTime.Now;
                option.CreatedBy = User.Identity.Name; // Set CreatedBy from logged-in user
                _context.Options.Add(option);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        // GET: Option/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var option = _context.Options.Find(id);
            if (option == null)
            {
                return NotFound();
            }
            return PartialView("_EditOptionPartial", option);
        }

        // POST: Option/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Option option)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    option.UpdatedDate = DateTime.Now;
                    option.UpdatedBy = User.Identity.Name; // Set UpdatedBy from logged-in user
                    _context.Update(option);
                    _context.SaveChanges();
                    _logger.LogInformation("Option with ID {optionId} updated successfully.", option.OptionsID);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating option with ID {optionId}.", option.OptionsID);
                    return Json(new { success = false, message = "An error occurred while updating the option." });
                }
            }

            var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            _logger.LogError("Failed to update option with ID {optionId}. Model state is invalid.", option.OptionsID);

            return Json(new { success = false, errors = errorMessages });
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var option = _context.Options.Find(id);
            if (option != null)
            {
                _context.Options.Remove(option);
                _context.SaveChanges();
                _logger.LogInformation("Option with ID {OptionId} deleted successfully.", id);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = new[] { "Option not found." } });
        }
    }
}
