using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task GetAllProductsQueryAsync(ProductListViewModel queryModel);

        Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId);
    }
}
