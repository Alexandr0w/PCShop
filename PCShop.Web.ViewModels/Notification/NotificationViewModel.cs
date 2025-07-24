namespace PCShop.Web.ViewModels.Notification
{
    public class NotificationViewModel
    {
        public required string Id { get; set; }

        public required string Message { get; set; } 

        public required string CreatedOn { get; set; }

        public bool IsRead { get; set; }
    }
}
