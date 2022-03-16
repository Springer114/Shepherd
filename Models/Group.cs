using System.ComponentModel.DataAnnotations;

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