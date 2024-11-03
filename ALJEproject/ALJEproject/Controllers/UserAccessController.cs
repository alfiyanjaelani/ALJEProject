using ALJEproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using ALJEproject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ALJEproject.Controllers
{
    public class UserAccessController : Controller
    {
        private readonly ILogger<UserAccessController> _logger;
        private readonly ALJEprojectDbContext _context;

        // Constructor to combine both dependencies
        public UserAccessController(ALJEprojectDbContext context, ILogger<UserAccessController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: UserAccess
        public async Task<IActionResult> Index()
        {
            var userAccessList = await _context.UserAccessesView.ToListAsync();
            return View(userAccessList);
        }

        // GET: UserAccess/Create
        [HttpGet]
        public IActionResult Create()
        {
            var roles = _context.Roles.Select(r => new { r.RoleID, r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleID", "RoleName");

            var menus = _context.Menus.Select(m => new { m.MenuID, m.MenuName }).ToList();
            ViewBag.MenuList = new SelectList(menus, "MenuID", "MenuName");

            return PartialView("_CreateUserAccessPartial");
        }

        // POST: UserAccess/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserAccess userAccess)
        {
            if (ModelState.IsValid)
            {
                userAccess.CreatedDate = DateTime.Now;
                userAccess.CreatedBy = User.Identity.Name; // Assuming you want to use the current user's name
                _context.UserAccesses.Add(userAccess);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        // GET: UserAccess/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userAccess = _context.UserAccesses.Find(id);
            if (userAccess == null)
            {
                return NotFound();
            }

            var roles = _context.Roles.Select(r => new { r.RoleID, r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleID", "RoleName", userAccess.RoleID);

            var menus = _context.Menus.Select(m => new { m.MenuID, m.MenuName }).ToList();
            ViewBag.MenuList = new SelectList(menus, "MenuID", "MenuName", userAccess.MenuID);

            return PartialView("_EditUserAccessPartial", userAccess);
        }

        // POST: UserAccess/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserAccess userAccess)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userAccess.UpdatedDate = DateTime.Now;
                    userAccess.UpdatedBy = User.Identity.Name; // Assuming you want to use the current user's name
                    _context.Update(userAccess);
                    _context.SaveChanges();
                    _logger.LogInformation("UserAccess with ID {UserAccessId} updated successfully.", userAccess.UserAccessID);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating UserAccess with ID {UserAccessId}.", userAccess.UserAccessID);
                    return Json(new { success = false, message = "An error occurred while updating the user access." });
                }
            }

            var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            _logger.LogError("Failed to update UserAccess with ID {UserAccessId}. Model state is invalid.", userAccess.UserAccessID);

            // If the model state is not valid, repopulate the dropdowns
            var roles = _context.Roles.Select(r => new { r.RoleID, r.RoleName }).ToList();
            ViewBag.RoleList = new SelectList(roles, "RoleID", "RoleName", userAccess.RoleID);

            var menus = _context.Menus.Select(m => new { m.MenuID, m.MenuName }).ToList();
            ViewBag.MenuList = new SelectList(menus, "MenuID", "MenuName", userAccess.MenuID);

            return Json(new { success = false, errors = errorMessages });
        }

        // POST: UserAccess/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userAccess = _context.UserAccesses.Find(id);
            if (userAccess != null)
            {
                _context.UserAccesses.Remove(userAccess);
                _context.SaveChanges();
                _logger.LogInformation("UserAccess with ID {UserAccessId} deleted successfully.", id);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = new[] { "UserAccess not found." } });
        }
    }
}
