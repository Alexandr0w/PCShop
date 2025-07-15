using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Computer;
using static PCShop.GCommon.ApplicationConstants;
using static PCShop.Services.Common.ExceptionMessages;

public class ComputerService : IComputerService
{
    private readonly IComputerRepository _computerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ComputerService(IComputerRepository computerRepository, UserManager<ApplicationUser> userManager)
    {
        this._computerRepository = computerRepository;
        this._userManager = userManager;
    }

    public async Task<IEnumerable<ComputerIndexViewModel>> GetAllComputersAsync(string? userId)
    {
        IEnumerable<ComputerIndexViewModel> computers = await this._computerRepository
            .GetAllAttached()
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
                    Price = c.Price
                })
                .SingleOrDefaultAsync();
        }

        return detailsComputerVM;
    }

    public async Task<bool> AddComputerAsync(string? userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException(UserIdNullOrEmpty);
        }

        bool isAdded = false;

        ApplicationUser? user = await this._userManager
            .FindByIdAsync(userId);

        string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

        if (user != null)
        {
            Computer computer = new Computer
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
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
            throw new FormatException(InvalidComputerIdFormat);
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
                ImageUrl = computerToEdit.ImageUrl
            };
        }

        return editModel;
    }

    public async Task<bool> PersistUpdatedComputerAsync(string userId, ComputerFormInputModel inputModel, IFormFile? imageFile)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException(UserIdNullOrEmpty);
        }

        if (!Guid.TryParse(inputModel.Id, out Guid computerId))
        {
            throw new FormatException(InvalidComputerIdFormat);
        }

        bool isPersisted = false;

        ApplicationUser? user = await this._userManager
            .FindByIdAsync(userId);

        Computer? updatedComputer = await this._computerRepository
            .GetByIdAsync(computerId);

        string imageUrl = await this.UploadImageAsync(inputModel, imageFile);

        if (user != null && updatedComputer != null)
        {
            updatedComputer.Name = inputModel.Name;
            updatedComputer.Description = inputModel.Description;
            updatedComputer.Price = inputModel.Price;
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
            throw new ArgumentException(UserIdNullOrEmpty);
        }

        if (!Guid.TryParse(computerId, out Guid computerIdGuid))
        {
            throw new FormatException(InvalidComputerIdFormat);
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
            throw new ArgumentException(UserIdNullOrEmpty);
        }

        if (!Guid.TryParse(inputModel.Id, out Guid computerId))
        {
            throw new FormatException(InvalidComputerIdFormat);
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
        string imageUrl = inputModel.ImageUrl ?? string.Empty;

        if (imageFile != null && imageFile.Length > 0)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
            string fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension)) throw new InvalidOperationException(InvalidFileType);
            if (!imageFile.ContentType.StartsWith("image/")) throw new InvalidOperationException(InvalidContentType);

            string uploadsFolder = Path.Combine(RootFolder, ImagesFolder, ComputersFolder);
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid() + fileExtension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream fileStream = new(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            imageUrl = $"/{ImagesFolder}/{ComputersFolder}/" + uniqueFileName;
        }

        return imageUrl;
    }
}
