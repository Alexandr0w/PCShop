namespace PCShop.Web.ViewModels.Admin.Product
{
    public class DeletedProductViewModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
