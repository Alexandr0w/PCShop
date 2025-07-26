using Microsoft.EntityFrameworkCore;
using Moq;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Tests.Helpers;
using PCShop.Web.ViewModels.Computer;
using System.Globalization;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class ComputerServiceTests
    {
        private Mock<IComputerRepository> _mockComputerRepo;
        private ComputerService _computerService;

        [SetUp]
        public void Setup()
        {
            this._mockComputerRepo = new Mock<IComputerRepository>();
            this._computerService = new ComputerService(_mockComputerRepo.Object);
        }

        [Test]
        public async Task GetAllComputersQueryAsync_ShouldFilterSearchTerm_AndSortByPriceDesc()
        {
            // Arrange
            var computers = new List<Computer>
            {
                new Computer { Id = Guid.NewGuid(), Name = "Gaming PC", Price = 2000, Description = "High-end gaming computer", ImageUrl = "pc.jpg", CreatedOn = DateTime.UtcNow.AddDays(-1), IsDeleted = false },
                new Computer { Id = Guid.NewGuid(), Name = "Office PC", Price = 1500, Description = "gaming computer", ImageUrl = "pc2.jpg", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new Computer { Id = Guid.NewGuid(), Name = "Workstation", Price = 3000, Description = "High-end gaming computer 3", ImageUrl = "pc3.jpg", CreatedOn = DateTime.UtcNow.AddDays(-2), IsDeleted = true } // Deleted
            };

            var mockDbSet = CreateMockDbSet(computers.Where(c => !c.IsDeleted).AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            var model = new ComputerListViewModel
            {
                SearchTerm = "Gaming",
                SortOption = "price_desc",
                CurrentPage = 1,
                ComputersPerPage = 10
            };

            // Act
            await this._computerService.GetAllComputersQueryAsync(model);

            // Assert
            Assert.That(model.TotalComputers, Is.EqualTo(1));
            Assert.That(model.Computers.Count(), Is.EqualTo(1));
            Assert.That(model.Computers.First().Name, Is.EqualTo("Gaming PC"));
        }

        [Test]
        public async Task GetAllComputersQueryAsync_ShouldPaginateResults()
        {
            // Arrange
            var computers = Enumerable.Range(1, 15)
                .Select(i => new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = $"PC {i}",
                    Price = 1000 + i,
                    Description = "High-end gaming computer",
                    ImageUrl = "test.jpg",
                    CreatedOn = DateTime.UtcNow.AddDays(-i),
                    IsDeleted = false
                })
                .ToList();

            var mockDbSet = CreateMockDbSet(computers.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            var model = new ComputerListViewModel
            {
                CurrentPage = 2,
                ComputersPerPage = 5
            };

            // Act
            await this._computerService.GetAllComputersQueryAsync(model);

            // Assert
            Assert.That(model.TotalComputers, Is.EqualTo(15));
            Assert.That(model.Computers.Count(), Is.EqualTo(5));
            Assert.That(model.Computers.First().Name, Is.EqualTo("PC 6"));
        }

        [Test]
        public async Task GetAllComputersQueryAsync_ShouldSortByNameAsc()
        {
            // Arrange
            var computers = new List<Computer>
            {
                new Computer { Id = Guid.NewGuid(), Name = "Zeta PC", Price = 2000, Description = "High-end gaming computer", ImageUrl = "pc.jpg", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new Computer { Id = Guid.NewGuid(), Name = "Alpha PC", Price = 1500, Description = "High-end gaming computer 2", ImageUrl = "pc2.jpg", CreatedOn = DateTime.UtcNow, IsDeleted = false }
            };

            var mockDbSet = CreateMockDbSet(computers.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            var model = new ComputerListViewModel
            {
                SortOption = "name_asc",
                CurrentPage = 1,
                ComputersPerPage = 10
            };

            // Act
            await this._computerService.GetAllComputersQueryAsync(model);

            // Assert
            Assert.That(model.Computers.First().Name, Is.EqualTo("Alpha PC"));
        }

        [Test]
        public async Task GetComputerDetailsAsync_WithValidGuid_ReturnsDetails()
        {
            // Arrange
            var computerId = Guid.NewGuid();
            var createdOn = DateTime.UtcNow;
            var computer = new Computer
            {
                Id = computerId,
                Name = "Test Computer",
                Description = "Test Description",
                Price = 1000,
                CreatedOn = createdOn,
                ImageUrl = "test.jpg",
                IsDeleted = false
            };

            var mockDbSet = CreateMockDbSet(new List<Computer> { computer }.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            // Act
            var result = await this._computerService.GetComputerDetailsAsync(null, computerId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Test Computer"));
            Assert.That(result.Description, Is.EqualTo("Test Description"));
            Assert.That(result.Price, Is.EqualTo(1000));
            Assert.That(result.ImageUrl, Is.EqualTo("test.jpg"));

            string expectedFormattedDate = createdOn.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture);
            Assert.That(result.CreatedOn, Is.EqualTo(expectedFormattedDate));

            DateTime parsedDate = DateTime.ParseExact(result.CreatedOn, DateAndTimeDisplayFormat, CultureInfo.InvariantCulture);
            Assert.That(parsedDate.Date, Is.EqualTo(createdOn.Date)); 
        }

        [Test]
        public async Task GetComputerDetailsAsync_WithInvalidGuid_ReturnsNull()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Computer>().AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            // Act
            var result = await this._computerService.GetComputerDetailsAsync(null, "invalid-guid");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetComputerDetailsAsync_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Computer>().AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached()).Returns(mockDbSet.Object);

            // Act
            var result = await this._computerService.GetComputerDetailsAsync(null, Guid.NewGuid().ToString());

            // Assert
            Assert.That(result, Is.Null);
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
    }
}