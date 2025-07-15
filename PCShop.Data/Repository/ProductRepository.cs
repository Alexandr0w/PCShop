using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Data.Repository;
using PCShop.Data.Repository.Interfaces;

public class ProductRepository : BaseRepository<Product, Guid>, IProductRepository
{
    public ProductRepository(PCShopDbContext dbContext) 
        : base(dbContext) 
    {
    }

    public async Task<IEnumerable<Product>> GetAllNonDeletedWithTypeAsync()
    {
        return await this._DbSet
            .Where(p => !p.IsDeleted)
            .Include(p => p.ProductType)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Product?> GetDetailsByIdAsync(string id)
    {
        return await this._DbSet
            .Include(p => p.ProductType)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == id.ToLower() && !p.IsDeleted);
    }

    public async Task<Product?> GetByIdWithTypeAsync(string id)
    {
        return await this._DbSet
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == id.ToLower());
    }
}