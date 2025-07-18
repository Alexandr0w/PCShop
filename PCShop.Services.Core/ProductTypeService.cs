﻿using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Product;

public class ProductTypeService : IProductTypeService
{
    private readonly IProductTypeRepository _productTypeRepository;

    public ProductTypeService(IProductTypeRepository productTypeRepository)
    {
        this._productTypeRepository = productTypeRepository;
    }

    public async Task<IEnumerable<ProductTypeViewModel>> GetProductTypeMenuAsync()
    {
        return await this._productTypeRepository
            .GetAllProductTypeViewModelsAsync();
    }
}
