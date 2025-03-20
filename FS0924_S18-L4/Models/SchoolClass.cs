using System.ComponentModel.DataAnnotations;

namespace FS0924_S18_L4.Models
{
    public class SchoolClass
    {
        [Key]
        public Guid SchoolClassId { get; set; }

        [Required]
        public required string SchoolClassName { get; set; }

        public ICollection<ApplicationUser>? ApplicationUsers { get; set; }
    }
}
