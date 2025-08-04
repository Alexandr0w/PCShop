namespace PCShop.Web.ViewModels.Manager
{
    public class OrderSummaryViewModel
    {
        public required string Id { get; set; }
        public required string CustomerName { get; set; }
        public required string OrderDate { get; set; }
        public required string Status { get; set; } 
        public decimal TotalPrice { get; set; }
    }
}
