using PCShop.Web.ViewModels.Admin;

namespace PCShop.Services.Core.Interfaces
{
    public interface IAdminService
    {
        Task<DeletedProductsListViewModel> GetDeletedProductsAsync(int currentPage = 1, int pageSize = 12);

        Task<bool> RestoreProductAsync(string productId);

        Task<bool> DeleteProductPermanentlyAsync(string productId);
    }
}
