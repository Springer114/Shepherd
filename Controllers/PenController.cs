using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;
using System;
using System.Linq;

namespace Shepherd.Controllers
{
    public class PenController : Controller
    {
        private readonly MyContext _context;
        
        public PenController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("pen/new")]
        public IActionResult NewPen()
        {
            if (GetCurrentUser() == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("pen/create")]
        public IActionResult CreatePen(Pen newPen)
        {
            User CurrentUser = GetCurrentUser();
            if (ModelState.IsValid)
            {
                newPen.Shepherd = CurrentUser;
                _context.Add(newPen);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("NewPen");
        }

        [HttpGet("pen/allpens")]
        public IActionResult AllPens()
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.AllPens = _context.Pens
                .Include(s => s.Shepherd)
                .Include(t => t.Tickets)
                .Include(p => p.TeamMembers)
                .ToList();

            return View();
        }

        [HttpGet("pen/{PenId}")]
        public IActionResult SinglePen(int PenId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            Pen singlePen = _context.Pens
                .Include(s => s.Shepherd)
                .Include(t => t.Tickets)
                .Include(p => p.TeamMembers)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(p => p.PenId == PenId);
            return View("SinglePen", singlePen);
        }

        [HttpGet("pen/{PenId}/edit")]
        public IActionResult EditPen(int PenId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("pen/{PenId}/update")]
        public IActionResult UpdatePen(int PenId, Pen UpdatedPen)
        {
            Pen PenToUpdate = _context.Pens
                .FirstOrDefault(p => p.PenId == PenId);

            if (ModelState.IsValid)
            {
                PenToUpdate.PenName = UpdatedPen.PenName;
                PenToUpdate.PenDescription = UpdatedPen.PenDescription;
                PenToUpdate.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("SinglePen");
        }

        [HttpPost("pen/{PenId}/delete")]
        public IActionResult DeletePen(int PenId)
        {
            Pen PenToDelete = _context.Pens
                .SingleOrDefault(p => p.PenId == PenId);
            _context.Pens.Remove(PenToDelete);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("pen/{PenId}/join")]
        public IActionResult JoinPen(int PenId)
        {
            Team toJoin = new Team()
            {
                UserId = GetCurrentUser().UserId,
                PenId = PenId
            };

            _context.Add(toJoin);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("pen/{PenId}/leave")]
        public IActionResult LeavePen(int PenId)
        {
            Team toLeave = _context.Teams
                .FirstOrDefault(u => u.UserId == GetCurrentUser().UserId && u.PenId == PenId);

            _context.Remove(toLeave);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet("pen/penmanagement")]
        public IActionResult PenManagement()
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }
            
            ViewBag.AllPens = _context.Pens
                .Where(s => s.Shepherd.UserId == GetCurrentUser().UserId)
                .Include(t => t.Tickets)
                .Include(m => m.TeamMembers)
                    .ThenInclude(u => u.User)
                .ToList();

            return View();
        }

        [HttpPost("pen/{PenId}/addmember")]
        public IActionResult AddMember(int PenId, string email)
        {

            User userToAdd = _context.Users.FirstOrDefault(u => u.Email == email);

            Team toAdd = new Team()
            {
                UserId = userToAdd.UserId,
                PenId = PenId
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

        public Pen GetCurrentPen(int PenId)
        {
            Pen CurrentPen = _context.Pens.First(p => p.PenId == PenId);
            return CurrentPen;
        }
    }
}