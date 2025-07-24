using Microsoft.EntityFrameworkCore;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Web.ViewModels.Admin.Computer;
using PCShop.Web.ViewModels.Admin.Product;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _productRepository;
        private readonly IComputerRepository _computerRepository;

        public AdminService(IProductRepository productRepository, IComputerRepository computerRepository)
        {
            this._productRepository = productRepository;
            this._computerRepository = computerRepository;
        }

        public async Task<DeletedProductsListViewModel> GetDeletedProductsAsync(int currentPage = 1, int pageSize = 12)
        {
            IQueryable<Product> query = this._productRepository
                .GetAllAttached()
                .Where(p => p.IsDeleted)
                .Include(p => p.ProductType)
                .IgnoreQueryFilters();

            int totalCount = await query.CountAsync();

            IEnumerable<DeletedProductViewModel> products = await query
                .OrderByDescending(p => p.DeletedOn) 
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new DeletedProductViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    DeletedOn = p.DeletedOn
                })
                .ToListAsync();

            return new DeletedProductsListViewModel
            {
                CurrentPage = currentPage,
                ProductsPerPage = pageSize,
                TotalProducts = totalCount,
                Products = products
            };
        }

        public async Task<bool> RestoreProductAsync(string productId)
        {
            bool isValidGuid = Guid.TryParse(productId, out Guid parsedId);

            if (!isValidGuid)
            {
                return false;
            }

            Product? product = await this._productRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == parsedId);

            if (product == null || !product.IsDeleted)
            {
                return false;
            }

            product.IsDeleted = false;
            product.DeletedOn = null;

            return await this._productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductPermanentlyAsync(string productId)
        {
            bool isValidGuid = Guid.TryParse(productId, out Guid parsedId);

            if (!isValidGuid)
            {
                return false;
            }

            Product? product = await this._productRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == parsedId);

            if (product == null || !product.IsDeleted)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), RootFolder, ImagesFolder, ProductsFolder, 
                    Path.GetFileName(product.ImageUrl));

                if (File.Exists(imagePath))
                {
                    try
                    {
                        File.Delete(imagePath);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Failed to delete product image '{imagePath}'.";
                        throw new IOException(errorMessage, ex);
                    }
                }
            }

            return await this._productRepository.HardDeleteAsync(product);
        }

        public async Task<DeletedComputersListViewModel> GetDeletedComputersAsync(int currentPage = 1, int pageSize = 12)
        {
            IQueryable<Computer> query = this._computerRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted);

            int totalCount = await query.CountAsync();

            IEnumerable<DeletedComputerViewModel> computers = await query
                .OrderByDescending(c => c.DeletedOn)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new DeletedComputerViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Price = c.Price,
                    DeletedOn = c.DeletedOn
                })
                .ToListAsync();

            return new DeletedComputersListViewModel
            {
                CurrentPage = currentPage,
                ProductsPerPage = pageSize,
                TotalProducts = totalCount,
                Computers = computers
            };
        }

        public async Task<bool> RestoreComputerAsync(string computerId)
        {
            bool isValidGuid = Guid.TryParse(computerId, out Guid parsedId);

            if (!isValidGuid)
            {
                return false;
            }

            Computer? computer = await this._computerRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == parsedId);

            if (computer == null || !computer.IsDeleted)
            {
                return false;
            }

            computer.IsDeleted = false;
            computer.DeletedOn = null;

            return await this._computerRepository.UpdateAsync(computer);
        }

        public async Task<bool> DeleteComputerPermanentlyAsync(string computerId)
        {
            bool isValidGuid = Guid.TryParse(computerId, out Guid parsedId);

            if (!isValidGuid)
            {
                return false;
            }

            Computer? computer = await this._computerRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == parsedId);

            if (computer == null || !computer.IsDeleted)
            {
                return false;
            }

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

            return await this._computerRepository.HardDeleteAsync(computer);
        }
    }
}
