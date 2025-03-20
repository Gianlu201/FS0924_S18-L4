using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L4.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public required string FirstName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public required string LastName { get; set; }

        [Required]
        [Display(Name = "Birth date")]
        public required DateOnly BirthDate { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
