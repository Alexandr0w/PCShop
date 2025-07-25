using static PCShop.Data.Common.EntityConstants.Product;

namespace PCShop.Web.ViewModels.Admin.ProductManagement
{
    public class ProductManagementPageViewModel
    {
        public int CurrentPage { get; set; } = ProductCurrentPage;

        public int ProductsPerPage { get; set; } = MaxProductsPerPage;
        
        public int TotalProducts { get; set; }

        public IEnumerable<ProductManagementIndexViewModel> Products { get; set; } = new List<ProductManagementIndexViewModel>();
    }
}
