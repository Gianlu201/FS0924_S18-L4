using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L4.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public required string Password { get; set; }
    }
}
