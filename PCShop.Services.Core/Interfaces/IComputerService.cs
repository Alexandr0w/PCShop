using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Computer;

namespace PCShop.Services.Core.Interfaces
{
    public interface IComputerService
    {
        Task<IEnumerable<ComputerIndexViewModel>> GetAllComputersAsync(string? userId);

        Task PopulateComputerQueryModelAsync(ComputerListViewModel model, string? userId);

        Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId);

        Task<bool> AddComputerAsync(string? userId, ComputerFormInputModel inputModel, IFormFile? imageFile);

        Task<ComputerFormInputModel?> GetComputerForEditingAsync(string computerId);
        Task<bool> PersistUpdatedComputerAsync(string userId, ComputerFormInputModel inputModel, IFormFile? imageFile);

        Task<DeleteComputerViewModel?> GetComputerForDeletingAsync(string? userId, string? computerId);
        Task<bool> SoftDeleteComputerAsync(string userId, DeleteComputerViewModel inputModel);

        Task<string> UploadImageAsync(ComputerFormInputModel inputModel, IFormFile? imageFile);
    }
}
