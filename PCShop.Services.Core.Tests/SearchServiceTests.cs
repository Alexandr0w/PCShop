using Microsoft.EntityFrameworkCore;
using PCShop.Data;
using PCShop.Data.Models;
using PCShop.Web.ViewModels.Search;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class SearchServiceTests
    {
        private PCShopDbContext _dbContext;
        private SearchService _searchService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<PCShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this._dbContext = new PCShopDbContext(options);
            this._searchService = new SearchService(_dbContext);

            SeedTestData();
        }

        [TearDown]
        public void TearDown()
        {
            this._dbContext.Dispose();
        }

        private void SeedTestData()
        {
            List<ProductType> productTypes = new List<ProductType>
            {
                new ProductType
                {
                    Id = Guid.NewGuid(),
                    Name = "Peripherals"
                },
                new ProductType
                {
                    Id = Guid.NewGuid(),
                    Name = "Cables"
                }
            };

            this._dbContext.ProductsTypes.AddRange(productTypes);
            this._dbContext.SaveChanges();

            ProductType peripheralsType = productTypes.First(pt => pt.Name == "Peripherals");
            ProductType cablesType = productTypes.First(pt => pt.Name == "Cables");

            List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Mouse",
                    Description = "High-performance gaming mouse with RGB lighting",
                    Price = 50.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/gaming-mouse.jpg",
                    IsDeleted = false,
                    ProductTypeId = peripheralsType.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Mechanical Keyboard",
                    Description = "Mechanical keyboard with blue switches",
                    Price = 120.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/keyboard.jpg",
                    IsDeleted = false,
                    ProductTypeId = peripheralsType.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Headset",
                    Description = "Surround sound gaming headset",
                    Price = 80.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/headset.jpg",
                    IsDeleted = false,
                    ProductTypeId = peripheralsType.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "USB Cable",
                    Description = "High-speed USB 3.0 cable",
                    Price = 15.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/usb-cable.jpg",
                    IsDeleted = false,
                    ProductTypeId = cablesType.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Wireless Mouse",
                    Description = "Wireless optical mouse",
                    Price = 35.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/wireless-mouse.jpg",
                    IsDeleted = false,
                    ProductTypeId = peripheralsType.Id
                }
            };

            List<Computer> computers = new List<Computer>
            {
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming PC Ultra",
                    Description = "High-end gaming computer with RTX 4080 and i9 processor",
                    Price = 1500.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/gaming-pc.jpg",
                    IsDeleted = false
                },
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Office Computer",
                    Description = "Reliable office computer for productivity tasks",
                    Price = 600.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/office-pc.jpg",
                    IsDeleted = false
                },
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Laptop",
                    Description = "Portable gaming laptop with dedicated graphics",
                    Price = 1200.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/gaming-laptop.jpg",
                    IsDeleted = false
                },
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Budget PC",
                    Description = "Affordable computer for basic computing needs",
                    Price = 400.00m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "https://example.com/budget-pc.jpg",
                    IsDeleted = false
                }
            };

            this._dbContext.Products.AddRange(products);
            this._dbContext.Computers.AddRange(computers);
            this._dbContext.SaveChanges();
        }

        [Test]
        public async Task SearchAsync_WithValidQuery_ReturnsMatchingResults()
        {
            // Arrange
            string query = "gaming";

            // Act
            SearchResultsViewModel result = await _searchService.SearchAsync(query);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query, Is.EqualTo(query));
            Assert.That(result.Results.Count, Is.EqualTo(4)); 
            Assert.That(result.TotalResults, Is.EqualTo(4));

            List<SearchResultItemViewModel> resultItems = result.Results.ToList();

            Assert.That(resultItems.Any(r => r.Name == "Gaming Mouse" && r.Type == "Product"), Is.True);
            Assert.That(resultItems.Any(r => r.Name == "Gaming Headset" && r.Type == "Product"), Is.True);
            Assert.That(resultItems.Any(r => r.Name == "Gaming PC Ultra" && r.Type == "Computer"), Is.True);
            Assert.That(resultItems.Any(r => r.Name == "Gaming Laptop" && r.Type == "Computer"), Is.True);
        }

        [Test]
        public async Task SearchAsync_WithCaseInsensitiveQuery_ReturnsMatchingResults()
        {
            // Arrange
            string query = "MOUSE";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            Assert.That(result.Results.Count, Is.EqualTo(2));
            Assert.That(result.Results.All(r => r.Name.ToLower().Contains("mouse")), Is.True);
        }

        [Test]
        public async Task SearchAsync_WithPartialMatch_ReturnsMatchingResults()
        {
            // Arrange
            string query = "PC";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            Assert.That(result.Results.Count, Is.EqualTo(2));

            List<string> resultNames = result.Results.Select(r => r.Name).ToList();

            Assert.That(resultNames, Contains.Item("Gaming PC Ultra"));
            Assert.That(resultNames, Contains.Item("Budget PC"));
        }

        [Test]
        public async Task SearchAsync_WithNoMatches_ReturnsEmptyResults()
        {
            // Arrange
            string query = "nonexistent";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            Assert.That(result.Results.Count, Is.EqualTo(0));
            Assert.That(result.TotalResults, Is.EqualTo(0));
            Assert.That(result.Query, Is.EqualTo(query));
        }

        [Test]
        public async Task SearchAsync_ResultsOrderedByPrice_ReturnsCorrectOrder()
        {
            // Arrange
            string query = "mouse";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            List<SearchResultItemViewModel> resultItems = result.Results.ToList();

            Assert.That(resultItems.Count, Is.EqualTo(2));
            Assert.That(resultItems[0].Price, Is.LessThan(resultItems[1].Price));
            Assert.That(resultItems[0].Name, Is.EqualTo("Wireless Mouse")); // Price: 35.00
            Assert.That(resultItems[1].Name, Is.EqualTo("Gaming Mouse")); // Price: 50.00
        }

        [Test]
        public async Task SearchAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange 
            ProductType additionalProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Products"
            };
            this._dbContext.ProductsTypes.Add(additionalProductType);
            await this._dbContext.SaveChangesAsync();

            List<Product> additionalProducts = new List<Product>();
            for (int i = 1; i <= 10; i++)
            {
                additionalProducts.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Test Product {i}",
                    Description = $"Description for test product {i}",
                    Price = i * 10.0m,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = $"https://example.com/test-product-{i}.jpg",
                    IsDeleted = false,
                    ProductTypeId = additionalProductType.Id
                });
            }
            this._dbContext.Products.AddRange(additionalProducts);
            await this._dbContext.SaveChangesAsync();

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync("test", currentPage: 2, itemsPerPage: 3);

            // Assert
            Assert.That(result.CurrentPage, Is.EqualTo(2));
            Assert.That(result.ItemsPerPage, Is.EqualTo(3));
            Assert.That(result.Results.Count, Is.EqualTo(3));
            Assert.That(result.TotalResults, Is.EqualTo(10));
        }

        [Test]
        public async Task SearchAsync_WithCustomItemsPerPage_ReturnsCorrectNumberOfItems()
        {
            // Arrange
            string query = "computer";
            int itemsPerPage = 2;

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query, itemsPerPage: itemsPerPage);

            // Assert
            Assert.That(result.ItemsPerPage, Is.EqualTo(itemsPerPage));
            Assert.That(result.Results.Count, Is.EqualTo(1)); // Only "Office Computer" contains "computer"
        }

        [Test]
        public async Task SearchAsync_WithCustomItemsPerPage_LimitsPagination()
        {
            string query = "gaming";
            int itemsPerPage = 2;

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query, itemsPerPage: itemsPerPage);

            // Assert
            Assert.That(result.ItemsPerPage, Is.EqualTo(itemsPerPage));

            int resultsCount = result.Results.Count();

            Assert.That(resultsCount, Is.LessThanOrEqualTo(itemsPerPage));
            Assert.That(result.TotalResults, Is.GreaterThanOrEqualTo(resultsCount));
        }

        [Test]
        public async Task SearchAsync_WithEmptyQuery_ReturnsAllResults()
        {
            // Arrange
            string query = "";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            // Should return all products and computers (5 products + 4 computers = 9 total)
            Assert.That(result.TotalResults, Is.EqualTo(9));
        }

        [Test]
        public async Task SearchAsync_ResultItemsHaveCorrectProperties()
        {
            // Arrange
            string query = "gaming mouse";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            SearchResultItemViewModel firstResult = result.Results.First();
            Assert.That(firstResult.Id, Is.Not.Null);
            Assert.That(firstResult.Id, Is.Not.Empty);
            Assert.That(firstResult.Name, Is.EqualTo("Gaming Mouse"));
            Assert.That(firstResult.Price, Is.EqualTo(50.00m));
            Assert.That(firstResult.Type, Is.EqualTo("Product"));
        }

        [Test]
        public async Task SearchAsync_WithHighPageNumber_ReturnsEmptyResults()
        {
            // Arrange
            string query = "gaming";
            int highPageNumber = 10;

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query, currentPage: highPageNumber);

            // Assert
            Assert.That(result.Results.Count, Is.EqualTo(0));
            Assert.That(result.CurrentPage, Is.EqualTo(highPageNumber));
            Assert.That(result.TotalResults, Is.GreaterThan(0)); 
        }

        [Test]
        public async Task SearchAsync_MixedProductsAndComputers_ReturnsBothTypes()
        {
            // Arrange
            string query = "gaming";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query);

            // Assert
            var productResults = result.Results.Where(r => r.Type == "Product").ToList();
            var computerResults = result.Results.Where(r => r.Type == "Computer").ToList();

            Assert.That(productResults.Count, Is.GreaterThan(0));
            Assert.That(computerResults.Count, Is.GreaterThan(0));

            List<decimal> allPrices = result.Results.Select(r => r.Price).ToList();
            List<decimal> sortedPrices = allPrices.OrderBy(p => p).ToList();

            Assert.That(allPrices, Is.EqualTo(sortedPrices));
        }


        [Test]
        public void SearchAsync_WithNullQuery_ThrowsException()
        {
            // Arrange
            string? query = null;

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await this._searchService.SearchAsync(query!));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task SearchAsync_WithInvalidCurrentPage_HandlesGracefully(int currentPage)
        {
            // Arrange
            string query = "gaming";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query, currentPage: currentPage);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CurrentPage, Is.EqualTo(currentPage));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task SearchAsync_WithInvalidItemsPerPage_HandlesGracefully(int itemsPerPage)
        {
            // Arrange
            string query = "gaming";

            // Act
            SearchResultsViewModel result = await this._searchService.SearchAsync(query, itemsPerPage: itemsPerPage);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ItemsPerPage, Is.EqualTo(itemsPerPage));
        }
    }
}