using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shepherd.Models
{
    public class Pen
    {
        [Key]

        public int PenId { get; set; }

        [Required(ErrorMessage = "Please provide a pen name.")]
        public string PenName { get; set; }

        [Required(ErrorMessage = "Please provide a description.")]
        public string PenDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public User Shepherd { get; set; }

        public List<Team> TeamMembers { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}