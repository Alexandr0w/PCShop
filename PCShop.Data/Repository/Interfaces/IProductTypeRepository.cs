using PCShop.Data.Models;
using PCShop.Web.ViewModels.Admin.ProductManagement;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IProductTypeRepository : IRepository<ProductType, Guid>, IAsyncRepository<ProductType, Guid>
    {
        Task<IEnumerable<ProductManagementProductTypeViewModel>> GetAllProductTypeViewModelsAsync();
    }
}
