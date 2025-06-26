using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Represents a part of a computer, linking it to a specific product")]
    public class ComputerPart
    {
        [Comment("Foreign key to the referenced Computer")]
        public Guid ComputerId { get; set; }
        public virtual Computer Computer { get; set; } = null!;

        [Comment("Foreign key to the referenced Product")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}