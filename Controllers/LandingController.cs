using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shepherd.Models;
using System.Linq;

namespace Shepherd.Controllers
{
    public class LandingController : Controller
    {
        private readonly MyContext _context;
        
        public LandingController(MyContext context)
        {
            _context = context;
        }

        [HttpGet ("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard", "Home");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            if (ModelState.IsValid)
            {
                var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.LoginEmail);
                if (foundUser == null)
                {
                    ModelState.AddModelError("LoginEmail", "Please check your email and password.");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Please check your email and password.");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", foundUser.UserId);
                return RedirectToAction("Dashboard", "Home");
            }
            return View("Index");
        }
    }
}