using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class UserTicket
    {
        [Key]
        public int UserTicketId { get; set; }
        public int UserId { get; set; }
        public User UserTicketAssigned { get; set; }
        public int TicketId { get; set; }
        public Ticket TicketAssigned { get; set; }
    }
}