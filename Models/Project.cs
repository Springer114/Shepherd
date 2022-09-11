using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shepherd.Models
{
    public class Project
    {
        [Key]

        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Please provide a Project name.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please provide a description.")]
        public string ProjectDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public User Shepherd { get; set; }

        public List<Team> TeamMembers { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}