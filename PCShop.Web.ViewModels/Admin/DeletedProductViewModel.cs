namespace PCShop.Web.ViewModels.Admin
{
    public class DeletedProductViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string ProductType { get; set; } = null!;

        public DateTime? DeletedOn { get; set; }
    }
}
