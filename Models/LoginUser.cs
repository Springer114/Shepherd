using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shepherd.Models
{
    public class LoginUser
    {

        [Required(ErrorMessage="Please provide your email.")]
        [EmailAddress(ErrorMessage="Please provide a valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage="Invalid email or password.")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword { get; set; }

    }
}