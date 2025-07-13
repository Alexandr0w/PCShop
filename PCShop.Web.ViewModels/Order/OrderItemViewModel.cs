namespace PCShop.Web.ViewModels.Order
{
    public class OrderItemViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string ImageUrl { get; set; }

        public required string OrderId { get; set; }

        public string? ProductId { get; set; }

        public string? ComputerId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
