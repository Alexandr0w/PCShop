using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents a computer in the PC Shop system")]
    public class Computer
    {
        [Comment("Unique identifier for the computer")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Name of the computer")]
        public required string Name { get; set; }

        [Comment("Description of the computer")]
        public required string Description { get; set; }

        [Comment("Price of the computer in decimal format")]
        public decimal Price { get; set; }

        [Comment("Image URL for the computer")]
        public required string ImageUrl { get; set; }

        [Comment("Indicates whether the computer is deleted")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<ComputerPart> ComputersParts { get; set; } = new HashSet<ComputerPart>();
        public virtual ICollection<OrderItem> OrdersItems { get; set; } = new HashSet<OrderItem>();
    }
}
