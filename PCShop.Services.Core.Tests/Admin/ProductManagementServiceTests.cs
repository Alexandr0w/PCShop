using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Admin;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Services.Core.Tests.Helpers;
using PCShop.Web.ViewModels.Admin.ProductManagement;
using System.Globalization;
using System.Text;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core.Tests.Admin
{
    [TestFixture]
    public class ProductManagementServiceTests
    {
        private Mock<IProductRepository> _mockProductRepo;
        private Mock<IProductTypeRepository> _mockProductTypeRepo;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private IProductManagementService _productManagementService;
        private const string TestRootFolder = "wwwroot";
        private const string TestImagesFolder = "images";
        private const string TestProductsFolder = "products";

        [SetUp]
        public void Setup()
        {
            this._mockProductRepo = new Mock<IProductRepository>();
            this._mockProductTypeRepo = new Mock<IProductTypeRepository>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            this._productManagementService = new ProductManagementService(
                this._mockProductRepo.Object,
                this._mockProductTypeRepo.Object,
                this._mockUserManager.Object);

            // Setup constants used in the service
            typeof(ProductManagementService)
                .GetField("RootFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestRootFolder);

            typeof(ProductManagementService)
                .GetField("ImagesFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestImagesFolder);

            typeof(ProductManagementService)
                .GetField("ProductsFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestProductsFolder);
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(Path.Combine(TestRootFolder, TestImagesFolder, TestProductsFolder)))
            {
                Directory.Delete(Path.Combine(TestRootFolder, TestImagesFolder, TestProductsFolder), true);
            }
        }

        private ApplicationUser CreateTestUser(string userId = null!)
        {
            return new ApplicationUser
            {
                Id = userId != null ? Guid.Parse(userId) : Guid.NewGuid(),
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                City = "Test City",
                Address = "Test Address",
                PostalCode = "1000",
                FullName = "Test User",
                PhoneNumber = "1234567890"
            };
        }

        private Product CreateTestProduct(Guid? id = null, bool isDeleted = false, Guid? productTypeId = null)
        {
            return new Product
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 1000,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = "test.jpg",
                IsDeleted = isDeleted,
                DeletedOn = isDeleted ? DateTime.UtcNow : null,
                ProductTypeId = productTypeId ?? Guid.NewGuid(),
                ProductType = new ProductType { Name = "Test Type" }
            };
        }

        private ProductType CreateTestProductType(Guid? id = null)
        {
            return new ProductType
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Test Type"
            };
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(data.Provider));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => data.GetEnumerator());

            return mockDbSet;
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsPagedResults()
        {
            // Arrange
            var productType = new ProductType { Name = "Test Type" };
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Product",
                    Description = "Test Description",
                    Price = 1000,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "test.jpg",
                    IsDeleted = false,
                    ProductType = productType,
                    ProductTypeId = productType.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Deleted Product",
                    Description = "Deleted Description",
                    Price = 2000,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    ImageUrl = "deleted.jpg",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                    ProductType = productType,
                    ProductTypeId = productType.Id
                }
            };

            var mockDbSet = CreateMockDbSet(products.AsQueryable());
            this._mockProductRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            var model = new ProductManagementPageViewModel
            {
                CurrentPage = 1,
                ProductsPerPage = 10
            };

            // Act
            await this._productManagementService.GetAllProductsAsync(model);

            // Assert
            Assert.That(model.TotalProducts, Is.EqualTo(2));
            Assert.That(model.Products.Count(), Is.EqualTo(2));
            Assert.That(model.Products.First().Name, Is.EqualTo("Test Product"));
            Assert.That(model.Products.First().ProductType, Is.EqualTo("Test Type"));
            Assert.That(model.Products.Last().IsDeleted, Is.True);
        }

        [Test]
        public async Task AddProductAsync_ValidInput_CreatesProduct()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var productTypeId = Guid.NewGuid();
            var inputModel = new ProductManagementFormInputModel
            {
                Name = "New Product",
                Description = "New Description",
                Price = 1500,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeInputFormat, CultureInfo.InvariantCulture),
                ProductTypeId = productTypeId.ToString(),
                ImageUrl = "default.jpg"
            };

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser(userId));
            this._mockProductTypeRepo.Setup(r => r.GetByIdAsync(productTypeId))
                .ReturnsAsync(CreateTestProductType(productTypeId));

            // Act
            var result = await this._productManagementService.AddProductAsync(userId, inputModel, null);

            // Assert
            Assert.IsTrue(result);
            this._mockProductRepo.Verify(r => r.AddAsync(It.Is<Product>(p =>
                p.Name == inputModel.Name &&
                p.ProductTypeId == productTypeId)),
                Times.Once);
        }

        [Test]
        public async Task GetProductEditFormModelAsync_ValidId_ReturnsModel()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = CreateTestProduct(productId);
            var productTypes = new List<ProductType> { CreateTestProductType() };
            var productTypeViewModels = new List<ProductManagementProductTypeViewModel>
            {
                new ProductManagementProductTypeViewModel { Id = product.ProductTypeId.ToString(), Name = "Test Type" }
            };

            this._mockProductRepo.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);
            this._mockProductTypeRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(productTypes);
            this._mockProductTypeRepo.Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(productTypeViewModels);

            // Act
            var result = await this._productManagementService.GetProductEditFormModelAsync(productId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.ProductTypes!.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task EditProductAsync_ValidInput_UpdatesProduct()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid();
            var productTypeId = Guid.NewGuid();

            var inputModel = new ProductManagementFormInputModel
            {
                Id = productId.ToString(),
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 2000,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture), 
                ProductTypeId = productTypeId.ToString(),
                ImageUrl = "/images/products/updated.jpg" 
            };

            var existingProduct = CreateTestProduct(productId);
            var productType = CreateTestProductType(productTypeId);

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser(userId));

            this._mockProductRepo.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            this._mockProductTypeRepo.Setup(r => r.GetByIdAsync(productTypeId))
                .ReturnsAsync(productType);

            // Act
            var result = await this._productManagementService.EditProductAsync(userId, inputModel, null);

            // Assert
            Assert.IsTrue(result);
            Assert.That(existingProduct.Name, Is.EqualTo(inputModel.Name));
            Assert.That(existingProduct.Description, Is.EqualTo(inputModel.Description));
            Assert.That(existingProduct.Price, Is.EqualTo(inputModel.Price));
            Assert.That(existingProduct.ProductTypeId, Is.EqualTo(productTypeId));
            this._mockProductRepo.Verify(r => r.UpdateAsync(existingProduct), Times.Once);
        }

        [Test]
        public async Task SoftDeleteProductAsync_ValidId_MarksAsDeleted()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = CreateTestProduct(productId);

            var mockDbSet = CreateMockDbSet(new List<Product> { product }.AsQueryable());
            this._mockProductRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._productManagementService.SoftDeleteProductAsync(productId.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(product.IsDeleted);
            Assert.NotNull(product.DeletedOn);
            this._mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Test]
        public async Task RestoreProductAsync_ValidId_RestoresProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = CreateTestProduct(productId, isDeleted: true);

            var mockDbSet = CreateMockDbSet(new List<Product> { product }.AsQueryable());
            this._mockProductRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._productManagementService.RestoreProductAsync(productId.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(product.IsDeleted);
            Assert.Null(product.DeletedOn);
            this._mockProductRepo.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Test]
        public async Task DeleteProductPermanentlyAsync_ValidId_DeletesProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = CreateTestProduct(productId, isDeleted: true);

            var mockDbSet = CreateMockDbSet(new List<Product> { product }.AsQueryable());
            this._mockProductRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._productManagementService.DeleteProductPermanentlyAsync(productId.ToString());

            // Assert
            Assert.IsTrue(result);
            this._mockProductRepo.Verify(r => r.HardDeleteAsync(product), Times.Once);
        }

        [Test]
        public async Task UploadImageAsync_WithValidImageFile_ReturnsNewImageUrl()
        {
            // Arrange
            var inputModel = new ProductManagementFormInputModel
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 1000,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeInputFormat, CultureInfo.InvariantCulture),
                ImageUrl = "old.jpg"
            };

            var fileName = "test.jpg";
            var content = "fake image content";
            var fileExtension = ".jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((target, token) => stream.CopyToAsync(target, token));

            // Act
            var result = await this._productManagementService.UploadImageAsync(inputModel, mockFile.Object);

            // Assert
            Assert.That(result, Does.StartWith($"/{TestImagesFolder}/{TestProductsFolder}/"));
            Assert.That(result, Does.EndWith(fileExtension));

            var fullPath = Path.Combine(TestRootFolder, result.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            Assert.That(File.Exists(fullPath), Is.True);
        }

        [Test]
        public void UploadImageAsync_WithInvalidFileExtension_ThrowsException()
        {
            // Arrange
            var inputModel = new ProductManagementFormInputModel
            {
                ImageUrl = "old.jpg"
            };

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                this._productManagementService.UploadImageAsync(inputModel, mockFile.Object));
        }

        [Test]
        public void UploadImageAsync_WithNoImage_ThrowsException()
        {
            // Arrange
            var inputModel = new ProductManagementFormInputModel
            {
                // No ImageUrl set
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _productManagementService.UploadImageAsync(inputModel, null));
        }
    }
}