using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductService
    {
        Task PopulateProductQueryModelAsync(ProductListViewModel queryModel, string? userId);

        Task<DetailsProductViewModel?> GetProductDetailsAsync(string? userId, string productId);

        Task<bool> AddProductAsync(string? userId, ProductFormInputModel inputModel, IFormFile? imageFile);

        Task<ProductFormInputModel?> GetProductForEditingAsync(string? productId);
        Task<bool> PersistUpdatedProductAsync(string userId, ProductFormInputModel inputModel, IFormFile? imageFile);

        Task<DeleteProductViewModel?> GetProductForDeletingAsync(string? userId, string? productId);
        Task<bool> SoftDeleteProductAsync(string userId, DeleteProductViewModel inputModel);

        Task<string> UploadImageAsync(ProductFormInputModel inputModel, IFormFile? imageFile);
    }
}
