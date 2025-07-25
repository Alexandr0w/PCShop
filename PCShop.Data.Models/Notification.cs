using Microsoft.EntityFrameworkCore;

namespace PCShop.Data.Models
{
    [Comment("Notification system for the project")]
    public class Notification
    {
        [Comment("Unique identifier for the notification")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Foreign key to the ApplicationUser receiving the notification")]
        public Guid ApplicationUserId { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [Comment("The title of the notification")]
        public required string Message { get; set; } 

        [Comment("The date and time when the notification was created")]
        public DateTime CreatedOn { get; set; }

        [Comment("Indicates whether the notification has been read by the user")]
        public bool IsRead { get; set; }
    }
}
