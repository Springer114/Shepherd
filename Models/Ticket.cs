using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shepherd.Models
{
    public class Ticket
    {
        [Key]

        public int TicketId { get; set; }

        [Required]
        public string TicketTitle { get; set; }

        public string TicketDescription { get; set; }

        public string TicketType { get; set; }

        public string TicketPriority { get; set; }

        public string TicketStatus { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public User Submitter { get; set; }

        public List<Group> GroupMembers { get; set; }

        public int ProjectId { get; set; }

        public Project HoldingProject { get; set; }
    }
}