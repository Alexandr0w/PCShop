using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Admin.ProductManagement;

namespace PCShop.Services.Core.Admin.Interfaces
{
    public interface IProductManagementService
    {
        Task<ProductManagementPageViewModel> GetAllProductsAsync(ProductManagementPageViewModel model);

        Task<bool> AddProductAsync(string? userId, ProductManagementFormInputModel inputModel, IFormFile? imageFile);

        Task<ProductManagementFormInputModel?> GetProductEditFormModelAsync(string? productId);
        Task<bool> EditProductAsync(string userId, ProductManagementFormInputModel inputModel, IFormFile? imageFile);

        Task<bool> SoftDeleteProductAsync(string? productId);
        Task<bool> RestoreProductAsync(string productId);
        Task<bool> DeleteProductPermanentlyAsync(string productId);

        Task<string> UploadImageAsync(ProductManagementFormInputModel inputModel, IFormFile? imageFile);
    }
}
