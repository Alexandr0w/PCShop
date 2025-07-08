using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string? userId);
    }
}
