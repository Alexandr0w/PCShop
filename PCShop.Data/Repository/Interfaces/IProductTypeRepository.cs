using PCShop.Data.Models;
using PCShop.Web.ViewModels.Product;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IProductTypeRepository : IRepository<ProductType, Guid>, IAsyncRepository<ProductType, Guid>
    {
        Task<IEnumerable<ProductTypeViewModel>> GetAllProductTypeViewModelsAsync();
    }
}
