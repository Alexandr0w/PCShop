using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models.Enum;

namespace PCShop.Data.Models
{
    [Comment("Represents an order placed by a user in the system")]
    public class Order
    {
        [Comment("Unique identifier for the order")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Foreign key to the referenced AspNetUser")]
        public Guid ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [Comment("The date and time when the order was placed")]
        public DateTime OrderDate { get; set; }

        [Comment("The current status of the order")]
        public OrderStatus Status { get; set; }

        [Comment("The total price of the order, including all items and taxes")]
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderItem> OrdersItems { get; set; } = new HashSet<OrderItem>();
    }
}
