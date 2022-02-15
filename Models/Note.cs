using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class Note
    {
        [Key]

        public int NoteId { get; set; }

        public string NoteMessage { get; set; }

        public int UserId { get; set; }

        public User NoteCreator { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}