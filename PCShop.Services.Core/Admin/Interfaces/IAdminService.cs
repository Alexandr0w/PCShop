using PCShop.Web.ViewModels.Admin.Computer;
using PCShop.Web.ViewModels.Admin.Product;

namespace PCShop.Services.Core.Admin.Interfaces
{
    public interface IAdminService
    {
        Task<DeletedProductsListViewModel> GetDeletedProductsAsync(int currentPage = 1, int pageSize = 12);
        Task<bool> RestoreProductAsync(string productId);
        Task<bool> DeleteProductPermanentlyAsync(string productId);

        Task<DeletedComputersListViewModel> GetDeletedComputersAsync(int currentPage = 1, int pageSize = 12);
        Task<bool> RestoreComputerAsync(string computerId);
        Task<bool> DeleteComputerPermanentlyAsync(string computerId);
    }
}
