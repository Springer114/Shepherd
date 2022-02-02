using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Flock
    {
        [Key]
        public int FlockId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PenId { get; set; }
        public Pen Pen { get; set; }
    }
}