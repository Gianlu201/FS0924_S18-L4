using Microsoft.AspNetCore.Identity;

namespace FS0924_S18_L4.Models
{
    public class ApplicationRole : IdentityRole
    {
        public required ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
