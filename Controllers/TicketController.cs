using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;

namespace Shepherd.Controllers
{
    public class TicketController : Controller
    {
        private readonly MyContext _context;
        
        public TicketController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("pen/{PenId}/ticket/new")]
        public IActionResult NewTicket(int PenId)
        {
            if (GetCurrentUser() == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.CurrentPen = GetCurrentPen(PenId);
            ViewBag.CurrentUser = GetCurrentUser();
            return View();
        }

        [HttpPost("{PenId}/ticket/create")]
        public IActionResult CreateTicket(Ticket newTicket, int PenId)
        {
            User CurrentUser = GetCurrentUser();
            Pen CurrentPen = GetCurrentPen(PenId);
            if (ModelState.IsValid)
            {
                newTicket.Submitter = CurrentUser;
                newTicket.HoldingPen = CurrentPen;
                _context.Add(newTicket);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View("NewTicket");
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
                .Include(p => p.GroupMembers)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(p => p.TicketId ==TicketId);
            return View("SingleTicket", singleTicket);
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

        public Pen GetCurrentPen(int PenId)
        {
            Pen CurrentPen = _context.Pens.First(p => p.PenId == PenId);
            return CurrentPen;
        }
    }
}