using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents a user in the PC Shop system")]
    public class ApplicationUser : IdentityUser
    {
        [Comment("Full name of the user")]
        public required string FullName { get; set; }

        [Comment("Full address of the user")]
        public required string Address { get; set; }

        [Comment("City where the user resides")]
        public required string City { get; set; }

        [Comment("State or region where the user resides")]
        public required string PostalCode { get; set; }

        [Comment("Indicates whether the user is deleted")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
