using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string TicketTitle { get; set; }
        public string TicketDescription { get; set;}
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public User TicketCreator { get; set; }
        public List<UserTicket> UsersOnTicket { get; set; }
        public int ProjectId { get; set; }
        public Project CurrentProject { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}