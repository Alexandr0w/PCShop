using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents a product in the PC Shop system")]
    public class Product
    {
        [Comment("Unique identifier for the product")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Name of the product")]
        public required string Name { get; set; }

        [Comment("Description of the product")]
        public required string Description { get; set; }

        [Comment("Price of the product in decimal format")]
        public decimal Price { get; set; }

        [Comment("Date and time when the product was created")]
        public DateTime CreatedOn { get; set; }

        [Comment("Image URL for the product")]
        public required string ImageUrl { get; set; }

        [Comment("Indicates whether the product is deleted")]
        public bool IsDeleted { get; set; }

        [Comment("Foreign key to the referenced ProductType")]
        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; } = null!;

        public virtual ICollection<ComputerPart> ComputersParts { get; set; } = new HashSet<ComputerPart>();
        public virtual ICollection<OrderItem> OrdersItems { get; set; } = new HashSet<OrderItem>();
    }
}