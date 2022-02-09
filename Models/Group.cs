using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}