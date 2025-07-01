using PCShop.Data.Models;

namespace PCShop.Services.Core.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<Product>> SearchAsync(string query);
    }
}
