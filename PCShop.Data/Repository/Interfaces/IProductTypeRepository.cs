using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IProductTypeRepository : IRepository<ProductType, Guid>, IAsyncRepository<ProductType, Guid>
    {
        Task<IEnumerable<ProductType>> GetAllAsViewModelsAsync();
    }
}
