using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;
using System;
using System.Linq;

namespace Shepherd.Controllers
{
    public class ProjectController : Controller
    {
        private readonly MyContext _context;
        
        public ProjectController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("Project/new")]
        public IActionResult NewProject()
        {
            if (GetCurrentUser() == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("Project/create")]
        public IActionResult CreateProject(Project newProject)
        {
            User CurrentUser = GetCurrentUser();
            if (ModelState.IsValid)
            {
                newProject.Shepherd = CurrentUser;
                _context.Add(newProject);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("NewProject");
        }

        [HttpGet("Project/allProjects")]
        public IActionResult AllProjects()
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.AllProjects = _context.Projects
                .Include(s => s.Shepherd)
                .Include(t => t.Tickets)
                .Include(p => p.TeamMembers)
                .ToList();

            return View();
        }

        [HttpGet("Project/{ProjectId}")]
        public IActionResult SingleProject(int ProjectId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            Project singleProject = _context.Projects
                .Include(s => s.Shepherd)
                .Include(t => t.Tickets)
                .Include(p => p.TeamMembers)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(p => p.ProjectId == ProjectId);
            return View("SingleProject", singleProject);
        }

        [HttpGet("Project/{ProjectId}/edit")]
        public IActionResult EditProject(int ProjectId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("Project/{ProjectId}/update")]
        public IActionResult UpdateProject(int ProjectId, Project UpdatedProject)
        {
            Project ProjectToUpdate = _context.Projects
                .FirstOrDefault(p => p.ProjectId == ProjectId);

            if (ModelState.IsValid)
            {
                ProjectToUpdate.ProjectName = UpdatedProject.ProjectName;
                ProjectToUpdate.ProjectDescription = UpdatedProject.ProjectDescription;
                ProjectToUpdate.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("SingleProject");
        }

        [HttpPost("Project/{ProjectId}/delete")]
        public IActionResult DeleteProject(int ProjectId)
        {
            Project ProjectToDelete = _context.Projects
                .SingleOrDefault(p => p.ProjectId == ProjectId);
            _context.Projects.Remove(ProjectToDelete);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("Project/{ProjectId}/join")]
        public IActionResult JoinProject(int ProjectId)
        {
            Team toJoin = new Team()
            {
                UserId = GetCurrentUser().UserId,
                ProjectId = ProjectId
            };

            _context.Add(toJoin);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("Project/{ProjectId}/leave")]
        public IActionResult LeaveProject(int ProjectId)
        {
            Team toLeave = _context.Teams
                .FirstOrDefault(u => u.UserId == GetCurrentUser().UserId && u.ProjectId == ProjectId);

            _context.Remove(toLeave);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet("Project/Projectmanagement")]
        public IActionResult ProjectManagement()
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }
            
            ViewBag.AllProjects = _context.Projects
                .Where(s => s.Shepherd.UserId == GetCurrentUser().UserId)
                .Include(t => t.Tickets)
                .Include(m => m.TeamMembers)
                    .ThenInclude(u => u.User)
                .ToList();

            return View();
        }

        [HttpPost("Project/{ProjectId}/addmember")]
        public IActionResult AddMember(int ProjectId, string email)
        {

            User userToAdd = _context.Users.FirstOrDefault(u => u.Email == email);

            Team toAdd = new Team()
            {
                UserId = userToAdd.UserId,
                ProjectId = ProjectId
            };

            _context.Add(toAdd);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
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

        public Project GetCurrentProject(int ProjectId)
        {
            Project CurrentProject = _context.Projects.First(p => p.ProjectId == ProjectId);
            return CurrentProject;
        }
    }
}