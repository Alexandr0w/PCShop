using Microsoft.EntityFrameworkCore;
using Moq;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Web.ViewModels.Product;
using PCShop.Services.Core.Tests.Helpers;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepo = null!;
        private ProductService _productService = null!;

        [SetUp]
        public void SetUp()
        {
            this._mockProductRepo = new Mock<IProductRepository>();
            this._productService = new ProductService(_mockProductRepo.Object);
        }

        [Test]
        public async Task GetProductDetailsAsync_WithInvalidGuid_ReturnsNull()
        {
            // Act
            var result = await this._productService.GetProductDetailsAsync(null, "invalid-guid");

            // Assert
            Assert.That(result, Is.Null);
            this._mockProductRepo.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task GetProductDetailsAsync_WithValidGuid_ReturnsDetails()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            var product = new Product
            {
                Id = guid,
                Name = "Test Product",
                Description = "Test Desc",
                Price = 100,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = "test.jpg",
                ProductType = new ProductType { Name = "GPU" }
            };

            var data = new List<Product> { product }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Product>>();

            mockDbSet.As<IAsyncEnumerable<Product>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Product>(data.GetEnumerator()));

            // Setup queryable
            mockDbSet.As<IQueryable<Product>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Product>(data.Provider));

            mockDbSet.As<IQueryable<Product>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);

            mockDbSet.As<IQueryable<Product>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);

            mockDbSet.As<IQueryable<Product>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => data.GetEnumerator());

            this._mockProductRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            // Act
            var result = await this._productService.GetProductDetailsAsync(null, guid.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Test Product"));
            Assert.That(result.ProductType, Is.EqualTo("GPU"));
        }

        [Test]
        public async Task GetAllProductsQueryAsync_ReturnsFilteredAndSortedProducts()
        {
            // Arrange
            ProductType productType = new ProductType { Name = "GPU" };
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "RTX 4090",
                    Description = "High-end NVIDIA GPU",
                    Price = 2000,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    ProductType = productType,
                    IsDeleted = false,
                    ImageUrl = "gpu.jpg"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "RX 7800",
                    Description = "AMD GPU",
                    Price = 1500,
                    CreatedOn = DateTime.UtcNow,
                    ProductType = productType,
                    IsDeleted = false,
                    ImageUrl = "gpu2.jpg"
                }
            };

            var mockDbSet = new Mock<DbSet<Product>>();
            var queryableProducts = products.AsQueryable();

            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Product>(queryableProducts.Provider));
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression)
                .Returns(queryableProducts.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType)
                .Returns(queryableProducts.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator())
                .Returns(() => queryableProducts.GetEnumerator());

            mockDbSet.As<IAsyncEnumerable<Product>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Product>(queryableProducts.GetEnumerator()));

            this._mockProductRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            ProductListViewModel model = new ProductListViewModel
            {
                ProductType = "GPU",
                SearchTerm = "RX",
                SortOption = "price_desc",
                CurrentPage = 1,
                ProductsPerPage = 10
            };

            // Act
            await this._productService.GetAllProductsQueryAsync(model);

            // Assert
            Assert.That(model.TotalProducts, Is.EqualTo(1));
            Assert.That(model.Products.Count(), Is.EqualTo(1));
            Assert.That(model.Products.First().Name, Is.EqualTo("RX 7800"));
            Assert.That(model.AllProductTypes.Contains("GPU"));
        }
    }
}