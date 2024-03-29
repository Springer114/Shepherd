﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shepherd.Models;
using System.Linq;

namespace Shepherd.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet ("home")]
        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Landing");
            }

            ViewBag.CurrentUser = GetCurrentUser();

            ViewBag.AllUserProjects = _context.Projects
                .Include(s => s.Shepherd)
                .Include(h => h.TeamMembers)
                .Include(t => t.Tickets)
                .OrderBy(c => c.CreatedAt)
                .ToList();
            
            ViewBag.AllUserTickets = _context.Tickets
                .Include(s => s.Submitter)
                .Include(g => g.GroupMembers)
                .Include(h => h.HoldingProject)
                .ToList();

            ViewBag.AllUserNotes = _context.Notes
                .Include(n => n.NoteCreator)
                .OrderBy(c => c.CreatedAt)
                .ToList();

            return View();
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Landing");
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

        [HttpPost("note/create")]
        public IActionResult Createnote(Note newNote)
        {
            User CurrentUser = GetCurrentUser();
            if (ModelState.IsValid)
            {
                newNote.NoteCreator = CurrentUser;
                _context.Add(newNote);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View("Dashboard");
        }

        [HttpPost("note/{NoteId}/delete")]
        public IActionResult DeleteNote(int NoteId)
        {
            Note NoteToDelete = _context.Notes
                .SingleOrDefault(n => n.NoteId == NoteId);
            _context.Notes.Remove(NoteToDelete);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }
    }
}