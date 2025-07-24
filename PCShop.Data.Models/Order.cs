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

        [Comment("Additional customer comment for the order")]
        public string? Comment { get; set; }

        [Comment("Delivery method for the order")]
        public DeliveryMethod DeliveryMethod { get; set; }

        [Comment("Payment method used for the order")]
        public OrderPaymentMethod PaymentMethod { get; set; }

        [Comment("Total delivery fee based on delivery method")]
        public decimal DeliveryFee { get; set; }

        [Comment("Final delivery address composed of city, postal code, and address")]
        public string? DeliveryAddress { get; set; }

        [Comment("Date and time when the order was sent to the user")]
        public DateTime? SendDate { get; set; }

        public virtual ICollection<OrderItem> OrdersItems { get; set; } = new HashSet<OrderItem>();
    }
}
