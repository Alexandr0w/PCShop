namespace PCShop.Web.ViewModels.Notification
{
    public class NotificationListViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; } = Enumerable.Empty<NotificationViewModel>();

        public int CurrentPage { get; set; } = 1;

        public int NotificationsPerPage { get; set; } = 10;

        public int TotalNotifications { get; set; }
    }

}
