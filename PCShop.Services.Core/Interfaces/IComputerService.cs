using PCShop.Web.ViewModels.Computer;

namespace PCShop.Services.Core.Interfaces
{
    public interface IComputerService
    {
        Task<ComputerListViewModel> GetAllComputersQueryAsync(ComputerListViewModel model);

        Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId);
    }
}
