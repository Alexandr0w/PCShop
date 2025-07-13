using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents an item in an order, linking a product to an order")]
    public class OrderItem
    {
        [Comment("Unique identifier for the ordered item")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Foreign key to the referenced Order")]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [Comment("Foreign key to the referenced Product")]
        public Guid? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Comment("Foreign key to the referenced Computer")]
        public Guid? ComputerId { get; set; }
        public virtual Computer? Computer { get; set; }

        [Comment("Number of units ordered")]
        public int Quantity { get; set; }
    }
}