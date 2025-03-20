using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FS0924_S18_L4.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public new required Guid UserId { get; set; }
        public new required Guid RoleId { get; set; }

        [ForeignKey("UserId")]
        public required ApplicationUser User { get; set; }

        [ForeignKey("RoleId")]
        public required ApplicationRole Role { get; set; }
    }
}
