using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please provide your first name.")]
        [MinLength(2, ErrorMessage = "First name must be at least two characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide your last name.")]
        [MinLength(2, ErrorMessage = "Last name must be at least two characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please provide your email address.")]
        [EmailAddress(ErrorMessage = "Must be a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a password.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Please ensure that the password and confirm password match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<Flock> Flocks { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}