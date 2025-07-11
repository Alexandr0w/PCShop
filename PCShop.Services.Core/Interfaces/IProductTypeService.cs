using PCShop.Web.ViewModels.Product;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeViewModel>> GetProductTypeMenuAsync();
    }
}
