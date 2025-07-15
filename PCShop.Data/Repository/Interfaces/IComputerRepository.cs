using PCShop.Data.Models;

namespace PCShop.Data.Repository.Interfaces
{
    public interface IComputerRepository : IRepository<Computer, Guid>, IAsyncRepository<Computer, Guid>
    {
    }
}
