using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Computer;

namespace PCShop.Services.Core.Interfaces
{
    public interface IComputerService
    {
        Task<IEnumerable<ComputerIndexViewModel>> GetAllComputersAsync(string? userId);

        Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId);

        Task<bool> AddComputerAsync(string? userId, AddComputerInputModel inputModel, IFormFile? imageFile);
    }
}
