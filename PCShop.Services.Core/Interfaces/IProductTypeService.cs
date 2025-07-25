using PCShop.Web.ViewModels.Admin.ProductManagement;

namespace PCShop.Services.Core.Interfaces
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductManagementProductTypeViewModel>> GetProductTypeMenuAsync();
    }
}
