using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Data.Repository;
using PCShop.Data.Repository.Interfaces;

public class ProductTypeRepository : BaseRepository<ProductType, Guid>, IProductTypeRepository
{
    public ProductTypeRepository(PCShopDbContext dbContext) 
        : base(dbContext) 
    {
    }

    public async Task<IEnumerable<ProductType>> GetAllAsViewModelsAsync()
    {
        return await this._DbSet
            .AsNoTracking()
            .ToArrayAsync();
    }
}