using System.ComponentModel.DataAnnotations;

namespace Shepherd.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}