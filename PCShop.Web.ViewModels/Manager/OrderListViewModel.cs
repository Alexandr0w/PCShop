namespace PCShop.Web.ViewModels.Manager
{
    public class OrderListViewModel
    {
        public required IEnumerable<OrderSummaryViewModel> Orders { get; set; }

        public int CurrentPage { get; set; }
        
        public int TotalPages { get; set; }
    }
}
