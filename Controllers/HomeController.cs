using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;

namespace Shepherd.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        
        public HomeController(MyContext context)
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
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        //Login the user

        [HttpPost("login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.Email);
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
            return RedirectToAction("Dashboard");
        }

        //Logout the user

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("home")]
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userId);
            return View();
        }
    }
}