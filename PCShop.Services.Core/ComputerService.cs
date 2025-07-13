using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core
{
    public class ComputerService : IComputerService
    {
        private readonly PCShopDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComputerService(PCShopDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
        }

        public async Task<IEnumerable<ComputerIndexViewModel>> GetAllComputersAsync(string? userId)
        {
            Guid? userGuid = null;
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedGuid))
            {
                userGuid = parsedGuid;
            }

            IEnumerable<ComputerIndexViewModel> computers = await this._dbContext
                .Computers
                .Include(c => c.ComputersParts)
                .AsNoTracking()
                .Select(c => new ComputerIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Description = c.Description,
                    Price = c.Price,
                    ImageUrl = c.ImageUrl
                })
                .ToArrayAsync();

            return computers;
        }

        public async Task<DetailsComputerViewModel?> GetComputerDetailsAsync(string? userId, string computerId)
        {
            DetailsComputerViewModel? computerDetails = null;

            Guid? userGuid = null;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedGuid))
            {
                userGuid = parsedGuid;
            }

            if (computerId != null)
            {
                Computer? computer = await this._dbContext
                    .Computers
                    .Include(c => c.ComputersParts)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id.ToString() == computerId);

                if (computer != null)
                {
                    computerDetails = new DetailsComputerViewModel
                    {
                        Id = computer.Id.ToString(),
                        Name = computer.Name,
                        Description = computer.Description,
                        ImageUrl = computer.ImageUrl,
                        Price = computer.Price
                    };
                }
            }

            return computerDetails;
        }

        public async Task<bool> AddComputerAsync(string? userId, AddComputerInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            string imageUrl = inputModel.ImageUrl ?? string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ComputersFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/{ImagesFolder}/{ComputersFolder}/" + uniqueFileName;
            }

            if (user != null)
            {
                Computer newComputer = new Computer
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    ImageUrl = imageUrl
                };

                await this._dbContext.AddAsync(newComputer);
                await this._dbContext.SaveChangesAsync();

                isAdded = true;
            }

            return isAdded;
        }
    }
}
