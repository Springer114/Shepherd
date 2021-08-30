using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class UserProject
    {
        [Key]
        public int UserProjectId { get; set; }
        public int UserId { get; set; }
        public User UserAssigned { get; set; }
        public int ProjectId { get; set; }
        public Project ProjectAssigned { get; set; }
    }
}