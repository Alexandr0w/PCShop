﻿using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Data.Repository;
using PCShop.Data.Repository.Interfaces;
using PCShop.Web.ViewModels.Admin.ProductManagement;

public class ProductTypeRepository : BaseRepository<ProductType, Guid>, IProductTypeRepository
{
    public ProductTypeRepository(PCShopDbContext dbContext)
        : base(dbContext) 
    {
    }

    public async Task<IEnumerable<ProductManagementProductTypeViewModel>> GetAllProductTypeViewModelsAsync()
    {
        return await this._DbSet
            .AsNoTracking()
            .Select(pt => new ProductManagementProductTypeViewModel
            {
                Id = pt.Id.ToString(),
                Name = pt.Name
            })
            .ToArrayAsync();
    }
}
