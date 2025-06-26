using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents an item in an order, linking a product to an order")]
    public class OrderItem
    {
        [Comment("Foreign key to the referenced Order")]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [Comment("Foreign key to the referenced Product")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}