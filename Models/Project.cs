using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Please provide a project name.")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please provide a description.")]
        public string ProjectDescription { get; set; }
        public int UserId { get; set; }
        public User ProjectCreator { get; set; }
        public List<User> ProjectUsers { get; set; }
        public List<Ticket> ProjectTickets { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}