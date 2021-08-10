using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Assignee
    {
        [Key]
        public int AssigneeId { get; set; }
        public int UserId { get; set; }
        public User UserAssignee { get; set; }
        public int ProjectId { get; set; }
        public Project ProjectAssignee { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}