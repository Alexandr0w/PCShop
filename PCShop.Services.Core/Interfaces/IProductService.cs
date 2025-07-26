using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductListViewModel> GetAllProductsQueryAsync(ProductListViewModel model);

        Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId);
    }
}
