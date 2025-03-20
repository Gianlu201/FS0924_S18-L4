using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L4.ViewModels
{
    public class EditUserByAdminViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public required string FirstName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public required string LastName { get; set; }

        public Guid? SchoolClassId { get; set; }
    }
}
