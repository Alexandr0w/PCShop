using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ExceptionMessages;

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
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            IEnumerable<ComputerIndexViewModel> computers = await this._dbContext
                .Computers
                .Include(c => c.ComputersParts)
                .AsNoTracking()
                .Where(p => p.IsDeleted == false)
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

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (string.IsNullOrEmpty(computerId))
            {
                throw new ArgumentException(ComputerIdNullOrEmpty);
            }

            if (computerId != null)
            {
                Computer? computer = await this._dbContext
                    .Computers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id.ToString() == computerId && !c.IsDeleted);

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

        public async Task<bool> AddComputerAsync(string? userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

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

        public async Task<ComputerFormInputModel?> GetComputerForEditingAsync(string computerId)
        {
            ComputerFormInputModel? editModel = null;

            Computer? computerToEdit = await this._dbContext
                .Computers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == Guid.Parse(computerId));

            if (computerToEdit != null)
            {
                editModel = new ComputerFormInputModel
                {
                    Id = computerToEdit.Id.ToString(),
                    Name = computerToEdit.Name,
                    Description = computerToEdit.Description,
                    Price = computerToEdit.Price,
                    ImageUrl = computerToEdit.ImageUrl
                };
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedComputerAsync(string userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isPersisted = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Computer? updatedComputer = await this._dbContext 
                .Computers
                .FindAsync(Guid.Parse(inputModel.Id));

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && updatedComputer != null)
            {
                updatedComputer.Name = inputModel.Name;
                updatedComputer.Description = inputModel.Description;
                updatedComputer.Price = inputModel.Price;
                updatedComputer.ImageUrl = imageUrl;

                await this._dbContext.SaveChangesAsync();
                isPersisted = true;
            }

            return isPersisted;
        }

        public async Task<DeleteComputerViewModel?> GetComputerForDeletingAsync(string? userId, string? computerId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (string.IsNullOrEmpty(computerId))
            {
                throw new ArgumentException(ComputerIdNullOrEmpty);
            }

            if (!Guid.TryParse(computerId, out Guid computerGuid))
            {
                throw new FormatException(InvalidComputerIdFormat);
            }

            DeleteComputerViewModel? deleteModel = null;

            Computer? deleteComputerModel = await this._dbContext
                .Computers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == computerGuid);

            if (deleteComputerModel != null)
            {
                deleteModel = new DeleteComputerViewModel
                {
                    Id = deleteComputerModel.Id.ToString(),
                    Name = deleteComputerModel.Name,
                    ImageUrl = deleteComputerModel.ImageUrl
                };
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteComputerAsync(string userId, DeleteComputerViewModel inputModel)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmpty);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid computerId))
            {
                throw new FormatException(InvalidComputerIdFormat);
            }

            bool isSuccessDeleted = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Computer? computer = await this._dbContext
                .Computers
                .FindAsync(computerId);

            if (user != null && computer != null)
            {
                computer.IsDeleted = true;
                isSuccessDeleted = true;

                await this._dbContext.SaveChangesAsync();
            }

            return isSuccessDeleted;
        }

        public async Task<string> UploadImageAsync(ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
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

            return imageUrl;
        }
    }
}
