using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Admin;
using PCShop.Services.Core.Admin.Interfaces;
using PCShop.Services.Core.Tests.Helpers;
using PCShop.Web.ViewModels.Admin.ComputerManagement;
using System.Globalization;
using System.Text;
using static PCShop.GCommon.ApplicationConstants;

namespace PCShop.Services.Core.Tests.Admin
{
    [TestFixture]
    public class ComputerManagementServiceTests
    {
        private Mock<IComputerRepository> _mockComputerRepo;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private IComputerManagementService _computerManagementService;
        private const string TestRootFolder = "wwwroot";
        private const string TestImagesFolder = "images";
        private const string TestComputersFolder = "computers";

        [SetUp]
        public void Setup()
        {
            this._mockComputerRepo = new Mock<IComputerRepository>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            this._computerManagementService = new ComputerManagementService(
                this._mockComputerRepo.Object,
                this._mockUserManager.Object);

            // Setup constants used in the service
            typeof(ComputerManagementService)
                .GetField("RootFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestRootFolder);

            typeof(ComputerManagementService)
                .GetField("ImagesFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestImagesFolder);

            typeof(ComputerManagementService)
                .GetField("ComputersFolder", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(null, TestComputersFolder);
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(Path.Combine(TestRootFolder, TestImagesFolder, TestComputersFolder)))
            {
                Directory.Delete(Path.Combine(TestRootFolder, TestImagesFolder, TestComputersFolder), true);
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

        private Computer CreateTestComputer(Guid? id = null, bool isDeleted = false)
        {
            return new Computer
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Test Computer",
                Description = "Test Description",
                Price = 1000,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = $"/{TestImagesFolder}/{TestComputersFolder}/test.jpg",
                IsDeleted = isDeleted,
                DeletedOn = isDeleted ? DateTime.UtcNow : null
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
        public async Task GetAllComputersAsync_ReturnsPagedResults()
        {
            // Arrange
            var computers = new List<Computer>
            {
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Computer",
                    Description = "Test Description",
                    Price = 1000,
                    CreatedOn = DateTime.UtcNow,
                    ImageUrl = "test.jpg",
                    IsDeleted = false
                },
                new Computer
                {
                    Id = Guid.NewGuid(),
                    Name = "Deleted Computer",
                    Description = "Deleted Description",
                    Price = 2000,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    ImageUrl = "deleted.jpg",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow
                }
            };

            var mockDbSet = CreateMockDbSet(computers.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            var model = new ComputerManagementPageViewModel
            {
                CurrentPage = 1,
                ComputersPerPage = 10
            };

            // Act
            await this._computerManagementService.GetAllComputersAsync(model);

            // Assert
            Assert.That(model.TotalComputers, Is.EqualTo(2));
            Assert.That(model.Computers.Count(), Is.EqualTo(2));
            Assert.That(model.Computers.First().Name, Is.EqualTo("Test Computer"));
            Assert.That(model.Computers.First().IsDeleted, Is.False);
            Assert.That(model.Computers.Last().Name, Is.EqualTo("Deleted Computer"));
            Assert.That(model.Computers.Last().IsDeleted, Is.True);
        }

        [Test]
        public async Task AddComputerAsync_ValidInput_CreatesComputer()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var inputModel = new ComputerManagementFormInputModel
            {
                Name = "New Computer",
                Description = "New Description",
                Price = 1500,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeInputFormat, CultureInfo.InvariantCulture),
                ImageUrl = "default.jpg"
            };

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser(userId));

            // Act
            var result = await this._computerManagementService.AddComputerAsync(userId, inputModel, mockFile.Object);

            // Assert
            Assert.IsTrue(result);
            this._mockComputerRepo.Verify(r => r.AddAsync(It.Is<Computer>(c =>
                c.Name == inputModel.Name)),
                Times.Once);
        }

        [Test]
        public async Task GetComputerEditFormModelAsync_ValidId_ReturnsModel()
        {
            // Arrange
            var computerId = Guid.NewGuid();
            var computer = CreateTestComputer(computerId);

            this._mockComputerRepo.Setup(r => r.GetByIdAsync(computerId))
                .ReturnsAsync(computer);

            // Act
            var result = await _computerManagementService.GetComputerEditFormModelAsync(computerId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(computer.Name));
            Assert.That(result.Description, Is.EqualTo(computer.Description));
            Assert.That(result.Price, Is.EqualTo(computer.Price));
            Assert.That(result.ImageUrl, Is.EqualTo(computer.ImageUrl));
        }

        [Test]
        public async Task EditComputerAsync_ValidInput_UpdatesComputer()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var computerId = Guid.NewGuid();
            var inputModel = new ComputerManagementFormInputModel
            {
                Id = computerId.ToString(),
                Name = "Updated Computer",
                Description = "Updated Description",
                Price = 2000,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeDisplayFormat, CultureInfo.InvariantCulture),
                ImageUrl = "/images/computers/updated.jpg" 
            };

            var mockFile = (IFormFile?)null; 

            var existingComputer = CreateTestComputer(computerId);

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser(userId));
            this._mockComputerRepo.Setup(r => r.GetByIdAsync(computerId))
                .ReturnsAsync(existingComputer);

            // Act
            var result = await this._computerManagementService.EditComputerAsync(userId, inputModel, mockFile);

            // Assert
            Assert.IsTrue(result);
            Assert.That(existingComputer.Name, Is.EqualTo(inputModel.Name));
            Assert.That(existingComputer.Description, Is.EqualTo(inputModel.Description));
            Assert.That(existingComputer.Price, Is.EqualTo(inputModel.Price));
            this._mockComputerRepo.Verify(r => r.UpdateAsync(existingComputer), Times.Once);
        }

        [Test]
        public async Task EditComputerAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var inputModel = new ComputerManagementFormInputModel
            {
                Id = Guid.NewGuid().ToString(),
                ImageUrl = "test-image.jpg"
            };

            this._mockComputerRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Computer?)null);

            // Act
            var result = await this._computerManagementService.EditComputerAsync(Guid.NewGuid().ToString(), inputModel, null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteComputerAsync_ValidId_MarksAsDeleted()
        {
            // Arrange
            var computerId = Guid.NewGuid();
            var computer = CreateTestComputer(computerId);

            var mockDbSet = CreateMockDbSet(new List<Computer> { computer }.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._computerManagementService.SoftDeleteComputerAsync(computerId.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(computer.IsDeleted);
            Assert.NotNull(computer.DeletedOn);
            this._mockComputerRepo.Verify(r => r.UpdateAsync(computer), Times.Once);
        }

        [Test]
        public async Task RestoreComputerAsync_ValidId_RestoresComputer()
        {
            // Arrange
            var computerId = Guid.NewGuid();
            var computer = CreateTestComputer(computerId, isDeleted: true);

            var mockDbSet = CreateMockDbSet(new List<Computer> { computer }.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._computerManagementService.RestoreComputerAsync(computerId.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(computer.IsDeleted);
            Assert.Null(computer.DeletedOn);
            this._mockComputerRepo.Verify(r => r.UpdateAsync(computer), Times.Once);
        }

        [Test]
        public async Task DeleteComputerPermanentlyAsync_ValidId_DeletesComputer()
        {
            // Arrange
            var computerId = Guid.NewGuid();
            var computer = CreateTestComputer(computerId, isDeleted: true);

            // Create the test image file
            var imagePath = Path.Combine(TestRootFolder, TestImagesFolder, TestComputersFolder, "test.jpg");
            Directory.CreateDirectory(Path.Combine(TestRootFolder, TestImagesFolder, TestComputersFolder));
            File.WriteAllText(imagePath, "test content");

            var mockDbSet = CreateMockDbSet(new List<Computer> { computer }.AsQueryable());
            this._mockComputerRepo.Setup(r => r.GetAllAttached())
                .Returns(mockDbSet.Object);

            // Act
            var result = await this._computerManagementService.DeleteComputerPermanentlyAsync(computerId.ToString());

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(File.Exists(imagePath));
            this._mockComputerRepo.Verify(r => r.HardDeleteAsync(computer), Times.Once);
        }

        [Test]
        public async Task UploadImageAsync_WithValidImageFile_ReturnsNewImageUrl()
        {
            // Arrange
            var inputModel = new ComputerManagementFormInputModel
            {
                Name = "Test Computer",
                Description = "Test Description",
                Price = 1000,
                CreatedOn = DateTime.UtcNow.ToString(DateAndTimeInputFormat, CultureInfo.InvariantCulture),
                ImageUrl = "old.jpg"
            };

            var fileName = "test.jpg";
            var fileExtension = ".jpg";
            var content = "fake image content for testing";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((target, token) => stream.CopyToAsync(target, token));

            // Act
            var result = await _computerManagementService.UploadImageAsync(inputModel, mockFile.Object);

            // Assert
            Assert.That(result, Does.StartWith($"/{TestImagesFolder}/{TestComputersFolder}/"));
            Assert.That(result, Does.EndWith(fileExtension));

            // Check if the image was actually saved
            var fullPath = Path.Combine(TestRootFolder, result.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            Assert.That(File.Exists(fullPath), Is.True);
        }

        [Test]
        public void UploadImageAsync_WithInvalidFileExtension_ThrowsException()
        {
            // Arrange
            var inputModel = new ComputerManagementFormInputModel
            {
                ImageUrl = "old.jpg"
            };

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.txt");
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                this._computerManagementService.UploadImageAsync(inputModel, mockFile.Object));
        }

        [Test]
        public void UploadImageAsync_WithNoImage_ThrowsException()
        {
            // Arrange
            var inputModel = new ComputerManagementFormInputModel
            {
                // No ImageUrl set
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _computerManagementService.UploadImageAsync(inputModel, null));
        }
    }
}
