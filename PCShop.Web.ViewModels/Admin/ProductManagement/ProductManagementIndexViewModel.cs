namespace PCShop.Web.ViewModels.Admin.ProductManagement
{
    public class ProductManagementIndexViewModel
    {
        public required string Id { get; set; } 

        public required string Name { get; set; } 

        public decimal Price { get; set; }

        public required string ProductType { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
