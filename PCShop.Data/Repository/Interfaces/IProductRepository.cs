using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IProductRepository : IRepository<Product, Guid>, IAsyncRepository<Product, Guid>
    {
        Task<IEnumerable<Product>> GetAllNonDeletedWithTypeAsync();

        Task<Product?> GetDetailsByIdAsync(string id);

        Task<Product?> GetByIdWithTypeAsync(string id);

        Task<Product?> GetAllProductsWithTypes(string productId, string productTypeId);
    }
}
