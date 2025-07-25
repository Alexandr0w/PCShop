namespace PCShop.Web.ViewModels.Admin.ProductManagement
{
    public class ProductManagementPageViewModel
    {
        public required IEnumerable<ProductManagementIndexViewModel> Products { get; set; }
        public int TotalProducts { get; set; }

        public int ProductsPerPage { get; set; }

        public int CurrentPage { get; set; }
    }
}
