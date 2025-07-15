using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;

namespace PCShop.Data.Repository
{
    public class ComputerRepository : BaseRepository<Computer, Guid>, IComputerRepository
    {
        public ComputerRepository(PCShopDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}
