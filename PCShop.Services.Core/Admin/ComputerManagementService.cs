using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.ComputerManagement;
using System.Globalization;
using static PCShop.GCommon.ExceptionMessages;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Admin
{
    public class ComputerManagementService : IComputerManagementService
    {
        private readonly IComputerRepository _computerRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComputerManagementService(IComputerRepository computerRepository, UserManager<ApplicationUser> userManager)
        {
            this._computerRepository = computerRepository;
            this._userManager = userManager;
        }

        public async Task<ComputerManagementPageViewModel> GetAllComputersAsync(ComputerManagementPageViewModel model)
        {
            IQueryable<Computer> computerQuery = this._computerRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking();

            model.TotalComputers = await computerQuery
                .CountAsync();

            model.Computers = await computerQuery
                .OrderByDescending(c => c.CreatedOn)
                .Skip((model.CurrentPage - 1) * model.ComputersPerPage)
                .Take(model.ComputersPerPage)
                .Select(c => new ComputerManagementIndexViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Price = c.Price,
                    CreatedOn = c.CreatedOn,
                    IsDeleted = c.IsDeleted,
                    DeletedOn = c.DeletedOn
                })
                .ToArrayAsync();

            return model;
        }

        public async Task<bool> AddComputerAsync(string? userId, ComputerManagementFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isAdded = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            bool isCreatedOnValid = DateTime
                        .TryParseExact(inputModel.CreatedOn, DateAndTimeInputFormat,
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

        public async Task<ComputerManagementFormInputModel?> GetComputerEditFormModelAsync(string? computerId)
        {
            ComputerManagementFormInputModel? editModel = null;

            if (!Guid.TryParse(computerId, out Guid computerIdGuid))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            Computer? computerToEdit = await this._computerRepository
                .GetByIdAsync(computerIdGuid);

            if (computerToEdit != null)
            {
                editModel = new ComputerManagementFormInputModel
                {
                    Id = computerToEdit.Id.ToString(),
                    Name = computerToEdit.Name,
                    Description = computerToEdit.Description,
                    Price = computerToEdit.Price,
                    CreatedOn = computerToEdit.CreatedOn.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture),
                    ImageUrl = computerToEdit.ImageUrl
                };
            }

            return editModel;
        }

        public async Task<bool> EditComputerAsync(string userId, ComputerManagementFormInputModel inputModel, IFormFile? imageFile)
        {
            bool isEdited = false;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException(UserIdNullOrEmptyMessage);
            }

            if (!Guid.TryParse(inputModel.Id, out Guid computerId))
            {
                throw new FormatException(InvalidComputerIdFormatMessage);
            }

            bool isCreatedOnValid = DateTime
                    .TryParseExact(inputModel.CreatedOn, DateAndTimeDisplayFormat,
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
                isEdited = true;
            }

            return isEdited;
        }

        public async Task<bool> SoftDeleteComputerAsync(string? computerId)
        {
            bool isAlreadyDeleted = false;

            if (!string.IsNullOrWhiteSpace(computerId))
            {
                Computer? computer = await this._computerRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(p => p.Id.ToString().ToLower() == computerId.ToString().ToLower());

                if (computer != null)
                {
                    computer.IsDeleted = true;
                    computer.DeletedOn = DateTime.UtcNow;

                    await this._computerRepository.UpdateAsync(computer);
                    isAlreadyDeleted = true;
                }
            }

            return isAlreadyDeleted;
        }

        public async Task<bool> RestoreComputerAsync(string computerId)
        {
            bool isRestored = false;

            if (!string.IsNullOrWhiteSpace(computerId))
            {
                Computer? computer = await this._computerRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == computerId.ToLower());

                if (computer != null && computer.IsDeleted)
                {
                    computer.IsDeleted = false;
                    computer.DeletedOn = null;

                    await this._computerRepository.UpdateAsync(computer);
                    isRestored = true;
                }
            }

            return isRestored;
        }

        public async Task<bool> DeleteComputerPermanentlyAsync(string computerId)
        {
            bool isPermanent = false;

            if (!string.IsNullOrWhiteSpace(computerId))
            {
                Computer? computer = await this._computerRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.Id.ToString().ToLower() == computerId.ToLower());

                if (computer != null && computer.IsDeleted)
                {
                    if (!string.IsNullOrEmpty(computer.ImageUrl))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), RootFolder, ImagesFolder, ComputersFolder,
                            Path.GetFileName(computer.ImageUrl));

                        if (File.Exists(imagePath))
                        {
                            try
                            {
                                File.Delete(imagePath);
                            }
                            catch (Exception ex)
                            {
                                string errorMessage = $"Failed to delete computer image '{imagePath}'.";
                                throw new IOException(errorMessage, ex);
                            }
                        }
                    }

                    await this._computerRepository.HardDeleteAsync(computer);
                    isPermanent = true;
                }
            }

            return isPermanent;
        }

        public async Task<string> UploadImageAsync(ComputerManagementFormInputModel inputModel, IFormFile? imageFile)
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
