using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;
using System;
using System.Linq;

namespace Shepherd.Controllers
{
    public class TicketController : Controller
    {
        private readonly MyContext _context;
        
        public TicketController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("Project/{ProjectId}/ticket/new")]
        public IActionResult NewTicket(int ProjectId)
        {
            if (GetCurrentUser() == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.CurrentProject = GetCurrentProject(ProjectId);
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("{ProjectId}/ticket/create")]
        public IActionResult CreateTicket(Ticket newTicket, int ProjectId)
        {
            User CurrentUser = GetCurrentUser();
            Project CurrentProject = GetCurrentProject(ProjectId);
            if (ModelState.IsValid)
            {
                newTicket.Submitter = CurrentUser;
                newTicket.HoldingProject = CurrentProject;
                _context.Add(newTicket);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("NewTicket");
        }

        [HttpGet("ticket/AllUserTickets")]
        public IActionResult AllUserTickets()
        {
            ViewBag.CurrentUser = GetCurrentUser();
            ViewBag.UserTickets = _context.Tickets
                .Include(h => h.HoldingProject)
                .Include(g => g.GroupMembers)
                .Include(s => s.Submitter)
                .ToList();
            return View();
        }

        [HttpGet("ticket/{TicketId}")]
        public IActionResult SingleTicket(int TicketId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            if (ViewBag.CurrentUser == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            Ticket singleTicket = _context.Tickets
                .Include(s => s.Submitter)
                .Include(g => g.GroupMembers)
                .FirstOrDefault(t => t.TicketId ==TicketId);
            return View("SingleTicket", singleTicket);
        }

        [HttpGet("ticket/{TicketId}/edit")]
        public IActionResult EditTicket(int TicketId)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("ticket/{TicketId}/update")]
        public IActionResult UpdateTicket(int TicketId, Ticket UpdatedTicket)
        {
            Ticket TicketToUpdate = _context.Tickets
                .FirstOrDefault(t => t.TicketId == TicketId);

            if (ModelState.IsValid)
            {
                TicketToUpdate.TicketTitle = UpdatedTicket.TicketTitle;
                TicketToUpdate.TicketDescription = UpdatedTicket.TicketDescription;
                TicketToUpdate.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("SingleTicket");
        }

        [HttpPost("ticket/{TicketId}/delete")]
        public IActionResult DeleteTicket(int TicketId)
        {
            Ticket TicketToDelete = _context.Tickets
                .SingleOrDefault(t => t.TicketId == TicketId);
            _context.Tickets.Remove(TicketToDelete);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("ticket/{TicketId}/join")]
        public IActionResult JoinTicket(int TicketId)
        {
            Group toJoin = new Group()
            {
                UserId = GetCurrentUser().UserId, TicketId = TicketId
            };

            _context.Add(toJoin);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("ticket/{TicketId}/leave")]
        public IActionResult LeaveTicket(int TicketId)
        {
            Group toLeave = _context.Groups
                .FirstOrDefault(u => u.UserId == GetCurrentUser().UserId && u.TicketId == TicketId);

            _context.Remove(toLeave);
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