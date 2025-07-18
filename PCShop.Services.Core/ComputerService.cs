using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core
{
    public class ComputerService : IComputerService
    {
        private readonly IComputerRepository _computerRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComputerService(IComputerRepository computerRepository, UserManager<ApplicationUser> userManager)
        {
            this._computerRepository = computerRepository;
            this._userManager = userManager;
        }

        public async Task PopulateComputerQueryModelAsync(ComputerListViewModel model, string? userId)
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

        public async Task<bool> AddComputerAsync(string? userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            bool isAdded = false;

            bool isCreatedOnValid = DateTime
                    .TryParseExact(inputModel.CreatedOn, CreatedOnFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOn);

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && isCreatedOnValid)
            {
                Computer computer = new Computer
                {
                    Name = inputModel.Name,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    CreatedOn = createdOn,
                    ImageUrl = imageUrl
                };

                await this._computerRepository.AddAsync(computer);

                isAdded = true;
            }
            return isAdded;
        }

        public async Task<ComputerFormInputModel?> GetComputerForEditingAsync(string computerId)
        {
            if (!Guid.TryParse(computerId, out Guid computerIdGuid))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            ComputerFormInputModel? editModel = null;

            Computer? computerToEdit = await this._computerRepository
                .GetByIdAsync(computerIdGuid);

            if (computerToEdit != null)
            {
                editModel = new ComputerFormInputModel
                {
                    Id = computerToEdit.Id.ToString(),
                    Name = computerToEdit.Name,
                    Description = computerToEdit.Description,
                    Price = computerToEdit.Price,
                    CreatedOn = computerToEdit.CreatedOn.ToString(CreatedOnFormat, CultureInfo.InvariantCulture),
                    ImageUrl = computerToEdit.ImageUrl
                };
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedComputerAsync(string userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid computerId))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            bool isPersisted = false;

            bool isCreatedOnValid = DateTime
                    .TryParseExact(inputModel.CreatedOn, CreatedOnFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdOn);

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Computer? updatedComputer = await this._computerRepository
                .GetByIdAsync(computerId);

            string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

            if (user != null && updatedComputer != null && isCreatedOnValid)
            {
                updatedComputer.Name = inputModel.Name;
                updatedComputer.Description = inputModel.Description;
                updatedComputer.Price = inputModel.Price;
                updatedComputer.CreatedOn = createdOn;
                updatedComputer.ImageUrl = imageUrl;

                await this._computerRepository.UpdateAsync(updatedComputer);

                isPersisted = true;
            }

            return isPersisted;
        }

        public async Task<DeleteComputerViewModel?> GetComputerForDeletingAsync(string? userId, string? computerId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(computerId, out Guid computerIdGuid))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            DeleteComputerViewModel? deleteModel = null;

            Computer? computerToDelete = await this._computerRepository
                .GetByIdAsync(computerIdGuid);

            if (computerToDelete != null)
            {
                deleteModel = new DeleteComputerViewModel
                {
                    Id = computerToDelete.Id.ToString(),
                    Name = computerToDelete.Name,
                    ImageUrl = computerToDelete.ImageUrl
                };
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteComputerAsync(string userId, DeleteComputerViewModel inputModel)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid computerId))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            bool isSuccessDeleted = false;

            ApplicationUser? user = await this._userManager
                .FindByIdAsync(userId);

            Computer? computer = await this._computerRepository
                .GetByIdAsync(computerId);

            if (user != null && computer != null)
            {
                await this._computerRepository.DeleteAsync(computer);

                isSuccessDeleted = true;
            }

            return isSuccessDeleted;
        }

        public async Task<string> UploadImageAsync(ComputerFormInputModel inputModel, IFormFile? imageFile)
        {
            string existingImageUrl = inputModel.ImageUrl ?? string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException(InvalidFileTypeMessage);
                }

                if (!imageFile.ContentType.StartsWith("image/"))
                {
                    throw new InvalidOperationException(InvalidContentTypeMessage);
                }

                // Delete old image if it exists 
                if (!string.IsNullOrWhiteSpace(existingImageUrl) && existingImageUrl.StartsWith($"/{ImagesFolder}/{ComputersFolder}/"))
                {
                    string oldImagePath = Path.Combine(RootFolder, existingImageUrl.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ComputersFolder);
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fileStream);

                existingImageUrl = $"/{ImagesFolder}/{ComputersFolder}/" + uniqueFileName;
            }

            if (string.IsNullOrWhiteSpace(existingImageUrl))
            {
                throw new InvalidOperationException(ImageNotFoundMessage);
            }

            return existingImageUrl;
        }
    }
}