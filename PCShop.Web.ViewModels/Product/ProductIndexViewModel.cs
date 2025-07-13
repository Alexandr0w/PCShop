namespace PCShop.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public required string Id { get; set; } 

        public required string Name { get; set; } 

        public required string ImageUrl { get; set; }

        public required string ProductType { get; set; } 

        public decimal Price { get; set; }
    }
}
