using Microsoft.AspNetCore.Http;
using PCShop.Web.ViewModels.Computer;

namespace PCShop.Services.Core.Interfaces
{
    public interface IComputerService
    {
        Task PopulateComputerQueryModelAsync(ComputerListViewModel model, string? userId);

        Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId);
    }
}
