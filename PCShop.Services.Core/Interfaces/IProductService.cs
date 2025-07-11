using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync(string? userId);

        Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId);

        Task<bool> AddProductAsync(string? userId, AddProductInputModel inputModel, IFormFile? imageFile);
    }
}
