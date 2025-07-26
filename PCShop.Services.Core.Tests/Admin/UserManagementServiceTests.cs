using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PCShop.Data.Models;
using PCShop.Services.Core.Admin;
using PCShop.Services.Core.Interfaces;
using PCShop.Services.Core.Tests.Helpers;
using PCShop.Web.ViewModels.Admin.UserManagement;

namespace PCShop.Services.Core.Tests.Admin
{
    [TestFixture]
    public class UserManagementServiceTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<RoleManager<IdentityRole<Guid>>> _mockRoleManager;
        private Mock<INotificationService> _mockNotificationService;
        private UserManagementService _userManagementService;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole<Guid>>>();
            this._mockRoleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            this._mockNotificationService = new Mock<INotificationService>();

            this._userManagementService = new UserManagementService(
                this._mockUserManager.Object,
                this._mockRoleManager.Object,
                this._mockNotificationService.Object);
        }

        private ApplicationUser CreateTestUser(bool isDeleted = false)
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "test@example.com",
                FullName = "Test Name",
                Address = "Test Address",
                City = "Test City",
                PostalCode = "1234",
                Email = "test@example.com",
                IsDeleted = isDeleted
            };
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsPagedResults()
        {
            // Arrange
            var users = new List<ApplicationUser>
            {
                CreateTestUser(),
                CreateTestUser(isDeleted: true)
            };

            var mockDbSet = CreateMockDbSet(users.AsQueryable());
            this._mockUserManager.Setup(x => x.Users).Returns(mockDbSet.Object);
            this._mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "User" });

            var model = new UserManagementPageViewModel
            {
                CurrentPage = 1,
                UsersPerPage = 10
            };

            // Act
            await this._userManagementService.GetAllUsersAsync(model);

            // Assert
            Assert.That(model.TotalUsers, Is.EqualTo(2));
            Assert.That(model.Users.Count(), Is.EqualTo(2));
            Assert.That(model.Users.First().Email, Is.EqualTo("test@example.com"));
            Assert.IsTrue(model.Users.Last().IsDeleted);
        }

        [Test]
        public async Task UserExistsByIdAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            this._mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser());

            // Act
            var result = await this._userManagementService.UserExistsByIdAsync(userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AssignUserToRoleAsync_ValidInput_AssignsRole()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var roleName = "Admin";
            var user = CreateTestUser();

            this._mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
            this._mockRoleManager.Setup(x => x.RoleExistsAsync(roleName))
                .ReturnsAsync(true);
            this._mockUserManager.Setup(x => x.IsInRoleAsync(user, roleName))
                .ReturnsAsync(false);
            this._mockUserManager.Setup(x => x.AddToRoleAsync(user, roleName))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await this._userManagementService.AssignUserToRoleAsync(userId, roleName);

            // Assert
            Assert.IsTrue(result);
            this._mockNotificationService.Verify(x => x.CreateAsync(user.Id.ToString(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task RemoveUserRoleAsync_ValidInput_RemovesRole()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var roleName = "Admin";
            var user = CreateTestUser();

            this._mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
            this._mockRoleManager.Setup(x => x.RoleExistsAsync(roleName))
                .ReturnsAsync(true);
            this._mockUserManager.Setup(x => x.IsInRoleAsync(user, roleName))
                .ReturnsAsync(true);
            this._mockUserManager.Setup(x => x.RemoveFromRoleAsync(user, roleName))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await this._userManagementService.RemoveUserRoleAsync(userId, roleName);

            // Assert
            Assert.IsTrue(result);
            this._mockNotificationService.Verify(x => x.CreateAsync(user.Id.ToString(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task DeleteUserAsync_ValidUser_SoftDeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser();

            this._mockUserManager.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
            this._mockUserManager.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await this._userManagementService.DeleteUserAsync(userId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(user.IsDeleted);
            this._mockNotificationService.Verify(x => x.CreateAsync(user.Id.ToString(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task RestoreUserAsync_ValidUser_RestoresUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser(isDeleted: true);
            user.Id = Guid.Parse(userId);

            var mockUsers = new List<ApplicationUser> { user }.AsQueryable();
            var mockDbSet = mockUsers.BuildMockDbSet();

            this._mockUserManager.Setup(x => x.Users).Returns(mockDbSet.Object);
            this._mockUserManager.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await this._userManagementService.RestoreUserAsync(userId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(user.IsDeleted);
            this._mockNotificationService.Verify(
                x => x.CreateAsync(user.Id.ToString(), "Your profile has been restored."),
                Times.Once);
        }

        [Test]
        public async Task DeleteUserForeverAsync_ValidUser_PermanentlyDeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser(isDeleted: true);
            user.Id = Guid.Parse(userId);

            var mockUsers = new List<ApplicationUser> { user }.AsQueryable();
            var mockDbSet = mockUsers.BuildMockDbSet();

            this._mockUserManager.Setup(x => x.Users).Returns(mockDbSet.Object);
            this._mockUserManager.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await this._userManagementService.DeleteUserForeverAsync(userId);

            // Assert
            Assert.IsTrue(result);
            this._mockUserManager.Verify(x => x.DeleteAsync(user), Times.Once);
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
