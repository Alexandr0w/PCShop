namespace PCShop.Web.ViewModels.Manager
{
    public class ManagerOrderViewModel
    {
        public required string Id { get; set; }

        public required string CustomerName { get; set; }

        public decimal TotalPrice { get; set; }

        public required string OrderDate { get; set; }

        public required string DeliveryMethod { get; set; }

        public required string Status { get; set; }

        public string? SendDate { get; set; }
    }
}