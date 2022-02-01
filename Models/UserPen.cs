using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class UserPen
    {
        [Key]
        public int UserPenId { get; set; }
        public int UserId { get; set; }
        public User UserAssigned { get; set; }
        public int PenId { get; set; }
        public Pen PenAssigned { get; set; }
    }
}