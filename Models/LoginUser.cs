using System.ComponentModel.DataAnnotations;

namespace Shepherd.Models
{
    public class LoginUser
    {

        [Required(ErrorMessage="Please provide your email.")]
        [EmailAddress(ErrorMessage="Please provide a valid email.")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage="Invalid email or password.")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}