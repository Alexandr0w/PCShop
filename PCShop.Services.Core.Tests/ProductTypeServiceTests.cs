using Moq;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Admin.ProductManagement;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class ProductTypeServiceTests
    {
        private Mock<IProductTypeRepository> _mockProductTypeRepository;
        private IProductTypeService _productTypeService;

        [SetUp]
        public void SetUp()
        {
            this._mockProductTypeRepository = new Mock<IProductTypeRepository>();
            this._productTypeService = new ProductTypeService(this._mockProductTypeRepository.Object);
        }

        [Test]
        public async Task GetProductTypeMenuAsync_WithProductTypes_ReturnsProductTypeViewModels()
        {
            // Arrange
            var expectedProductTypes = new List<ProductManagementProductTypeViewModel>
            {
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Graphics Cards"
                },
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Processors"
                },
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Memory"
                }
            };

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(expectedProductTypes);

            // Act
            var result = await this._productTypeService.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);

            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(3));

            var firstProductType = resultList.First();
            Assert.That(firstProductType.Id, Is.EqualTo(expectedProductTypes[0].Id));
            Assert.That(firstProductType.Name, Is.EqualTo("Graphics Cards"));

            var resultNames = resultList.Select(pt => pt.Name).ToList();
            Assert.That(resultNames, Contains.Item("Graphics Cards"));
            Assert.That(resultNames, Contains.Item("Processors"));
            Assert.That(resultNames, Contains.Item("Memory"));

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductTypeMenuAsync_WithEmptyRepository_ReturnsEmptyCollection()
        {
            // Arrange
            var emptyProductTypes = new List<ProductManagementProductTypeViewModel>();

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(emptyProductTypes);

            // Act
            var result = await _productTypeService.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductTypeMenuAsync_WithSingleProductType_ReturnsSingleItem()
        {
            // Arrange
            var singleProductType = new List<ProductManagementProductTypeViewModel>
            {
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Motherboards"
                }
            };

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(singleProductType);

            // Act
            var result = await this._productTypeService.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);

            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(1));
            Assert.That(resultList[0].Name, Is.EqualTo("Motherboards"));
            Assert.That(resultList[0].Id, Is.Not.Null);
            Assert.That(resultList[0].Id, Is.Not.Empty);

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public void GetProductTypeMenuAsync_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Database connection failed");
            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ThrowsAsync(expectedException);

            // Act & Assert
            var actualException = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await this._productTypeService.GetProductTypeMenuAsync());

            Assert.That(actualException.Message, Is.EqualTo("Database connection failed"));

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductTypeMenuAsync_ReturnsIEnumerable_CanBeEnumeratedMultipleTimes()
        {
            // Arrange
            var productTypes = new List<ProductManagementProductTypeViewModel>
            {
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Storage"
                },
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Power Supplies"
                }
            };

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(productTypes);

            // Act
            var result = await this._productTypeService.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);

            var firstEnumeration = result.ToList();
            var secondEnumeration = result.ToList();

            Assert.That(firstEnumeration.Count, Is.EqualTo(2));
            Assert.That(secondEnumeration.Count, Is.EqualTo(2));
            Assert.That(firstEnumeration[0].Name, Is.EqualTo(secondEnumeration[0].Name));
            Assert.That(firstEnumeration[1].Name, Is.EqualTo(secondEnumeration[1].Name));

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public async Task GetProductTypeMenuAsync_WithLargeDataSet_HandlesCorrectly()
        {
            // Arrange
            var largeProductTypeList = new List<ProductManagementProductTypeViewModel>();

            for (int i = 1; i <= 100; i++)
            {
                largeProductTypeList.Add(new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"Product Type {i}"
                });
            }

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(largeProductTypeList);

            // Act
            var result = await _productTypeService.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);

            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(100));

            Assert.That(resultList[0].Name, Is.EqualTo("Product Type 1"));
            Assert.That(resultList[99].Name, Is.EqualTo("Product Type 100"));

            Assert.That(resultList.All(pt => !string.IsNullOrEmpty(pt.Id)), Is.True);
            Assert.That(resultList.All(pt => !string.IsNullOrEmpty(pt.Name)), Is.True);

            this._mockProductTypeRepository.Verify(r => r.GetAllProductTypeViewModelsAsync(), Times.Once);
        }

        [Test]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductTypeService(null!));
        }

        [Test]
        public async Task GetProductTypeMenuAsync_ShouldReturn_ValidIEnumerable()
        {
            // Arrange
            var productTypes = new List<ProductManagementProductTypeViewModel>
            {
                new ProductManagementProductTypeViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Test Type"
                }
            };

            this._mockProductTypeRepository
                .Setup(r => r.GetAllProductTypeViewModelsAsync())
                .ReturnsAsync(productTypes);

            var service = new ProductTypeService(this._mockProductTypeRepository.Object);

            // Act
            var result = await service.GetProductTypeMenuAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductManagementProductTypeViewModel>>());

            var asList = result.ToList();
            var asArray = result.ToArray();

            Assert.That(asList.Count, Is.EqualTo(asArray.Length));
            Assert.That(asList[0].Name, Is.EqualTo("Test Type"));
        }
    }
}