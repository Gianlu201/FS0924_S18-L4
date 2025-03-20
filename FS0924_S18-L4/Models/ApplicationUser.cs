using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace FS0924_S18_L4.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required DateOnly BirthDate { get; set; }

        public Guid? SchoolClassId { get; set; }

        public ICollection<ApplicationUserRole>? ApplicationUserRole { get; set; }

        [ForeignKey("SchoolClassId")]
        public SchoolClass? SchoolClass { get; set; }
    }
}
