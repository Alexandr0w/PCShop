using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Product type of the PC Shop system")]
    public class ProductType
    {
        [Comment("Unique identifier for the product type")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Name of the product type")]
        public required string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
