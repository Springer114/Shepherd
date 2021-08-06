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
            ViewBag.AllProjects = _context.Projects
                .Include(a => a.ProjectCreator)
                .Include(p => p.ProjectTickets)
                .OrderBy(a => a.CreatedAt)
                .ToList();
            return View();
        }

        [HttpGet("project/new")]
        public IActionResult NewProject()
        {
            return View();
        }

        [HttpPost("project/create")]
        public IActionResult CreateProject(Project newProject)
        {
            if (ModelState.IsValid)
            {
                newProject.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Add(newProject);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("NewProject");
            }
        }

        [HttpGet("project/{id}")]
        public IActionResult ShowProject(int id)
        {
            ViewBag.CurrentProject = _context.Projects
                .Include(c => c.ProjectCreator)
                .Include(p => p.ProjectTickets)
                .FirstOrDefault(a => a.ProjectId == id);
            ViewBag.CurrentUser = _context.Users
                .FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            return View("ShowProject");
        }

        [HttpPost("project/{projectId}/delete")]
        public IActionResult DeleteProject(int ProjectId)
        {
            var ProjectToDelete = _context.Projects
                .First(a => a.ProjectId == ProjectId);
            _context.Remove(ProjectToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public User GetCurrentUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return null;
            }
            User CurrentUser = _context.Users.First(u => u.UserId == userId);
            return CurrentUser;
        }
    }
}