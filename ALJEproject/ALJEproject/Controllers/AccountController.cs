using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ALJEproject.Models; // Sesuaikan namespace berdasarkan struktur proyek Anda
using ALJEproject.ViewModels; // Anggap Anda memiliki LoginViewModel untuk detail login
using ALJEproject.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ALJEproject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ALJEprojectDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        // Konstruktor yang menggabungkan kedua dependensi
        public AccountController(ALJEprojectDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.Username) || string.IsNullOrEmpty(vm.Password))
            {
                ModelState.AddModelError("", "Username and password are required.");
                return View();
            }

            if (SignInMethod(vm.Username, vm.Password))
            {
                // Set session untuk username dan role
                HttpContext.Session.SetString("Username", vm.Username);

                // Misalkan Anda mendapatkan role dari database berdasarkan username
                var user = _context.UserRoles.SingleOrDefault(u => u.UserName == vm.Username);
                if (user != null)
                {
                    HttpContext.Session.SetString("Role", user.RoleName);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Menghapus semua data session
            return RedirectToAction("Login", "Account");
        }

        private bool SignInMethod(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return false; // User not found
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }      
    }
}
