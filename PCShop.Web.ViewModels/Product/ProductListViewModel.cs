using static PCShop.Data.Common.EntityConstants.Product;

namespace PCShop.Web.ViewModels.Product
{
    public class ProductListViewModel
    {
        public int CurrentPage { get; set; } = ProductCurrentPage;

        public int ProductsPerPage { get; set; } = MaxProductsPerPage;

        public int TotalProducts { get; set; }

        public string? SearchTerm { get; set; }

        public string? ProductType { get; set; }

        public string? SortOption { get; set; } = DefaultSortOption;

        public IEnumerable<ProductIndexViewModel> Products { get; set; } = new List<ProductIndexViewModel>();

        public IEnumerable<string> AllProductTypes { get; set; } = new List<string>();
    }
}
