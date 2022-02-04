using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PenId { get; set; }
        public Pen Pen { get; set; }
    }
}