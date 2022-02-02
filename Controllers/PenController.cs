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
                .Include(p => p.Herders)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(p => p.PenId == PenId);
            return View("SinglePen", singlePen);
        }

        [HttpPost("pen/{PenId}/join")]
        public IActionResult JoinPen(int PenId)
        {
            Flock toJoin = new Flock()
            {
                UserId = GetCurrentUser().UserId, PenId = PenId
            };

            _context.Add(toJoin);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpPost("pen/{PenId}/leave")]
        public IActionResult LeavePen(int PenId)
        {
            Flock toLeave = _context.Flocks
                .FirstOrDefault(u => u.UserId == GetCurrentUser().UserId && u.PenId == PenId);

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
    }
}