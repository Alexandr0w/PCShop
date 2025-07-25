using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task PopulateProductQueryModelAsync(ProductListViewModel queryModel, string? userId);

        Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId);
    }
}
