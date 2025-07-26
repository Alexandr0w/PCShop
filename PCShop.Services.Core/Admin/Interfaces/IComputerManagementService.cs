using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Admin.ComputerManagement;

namespace PCShop.Services.Core.Admin.Interfaces
{
    public interface IComputerManagementService
    {
        Task<ComputerManagementPageViewModel> GetAllComputersAsync(ComputerManagementPageViewModel model);

        Task<bool> AddComputerAsync(string? userId, ComputerManagementFormInputModel inputModel, IFormFile? imageFile);

        Task<ComputerManagementFormInputModel?> GetComputerEditFormModelAsync(string? computerId);
        Task<bool> EditComputerAsync(string userId, ComputerManagementFormInputModel inputModel, IFormFile? imageFile);

        Task<bool> SoftDeleteComputerAsync(string? computerId);
        Task<bool> RestoreComputerAsync(string computerId);
        Task<bool> DeleteComputerPermanentlyAsync(string computerId);

        Task<string> UploadImageAsync(ComputerManagementFormInputModel inputModel, IFormFile? imageFile);
    }
}
