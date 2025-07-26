using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _computerRepository;

        public ComputerService(IComputerRepository computerRepository)
        {
            this._computerRepository = computerRepository;
        }

        public async Task GetAllComputersQueryAsync(ComputerListViewModel model)
        {
            IQueryable<Computer> query = this._computerRepository
                .GetAllAttached()
                .Where(c => !c.IsDeleted)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                string term = model.SearchTerm.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(term));
            }

            query = model.SortOption switch
            {
                "name_asc" => query.OrderBy(c => c.Name),
                "name_desc" => query.OrderByDescending(c => c.Name),
                "price_asc" => query.OrderBy(c => c.Price),
                "price_desc" => query.OrderByDescending(c => c.Price),
                "oldest" => query.OrderBy(p => p.CreatedOn),
                "default" or null or "" => query.OrderByDescending(c => c.CreatedOn),
                _ => query.OrderByDescending(c => c.CreatedOn)
            };

            model.TotalComputers = await query.CountAsync();

            model.Computers = await query
                .Skip((model.CurrentPage - 1) * model.ComputersPerPage)
                .Take(model.ComputersPerPage)
                .Select(c => new ComputerIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl
                })
                .ToArrayAsync();
        }

        public async Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId)
        {
            DetailsComputerViewModel? detailsComputerVM = null;

            bool isIdValidGuid = Guid.TryParse(computerId, out Guid computerIdGuid);

            if (isIdValidGuid)
            {
                detailsComputerVM = await this._computerRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(c => c.Id == computerIdGuid)
                    .Select(c => new DetailsComputerViewModel()
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        Price = c.Price,
                        CreatedOn = c.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture)
                    })
                    .SingleOrDefaultAsync();
            }

            return detailsComputerVM;
        }
    }
}