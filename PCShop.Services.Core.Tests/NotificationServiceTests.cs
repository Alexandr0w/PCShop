using Moq;
using PCShop.Data.Models;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Tests.Helpers;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private Mock<INotificationRepository> _mockNotificationRepository;
        private NotificationService _notificationService;
        private readonly Guid _testUserId = Guid.NewGuid();
        private readonly Guid _testNotificationId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            this._mockNotificationRepository = new Mock<INotificationRepository>();
            this._notificationService = new NotificationService(_mockNotificationRepository.Object);
        }

        [Test]
        public async Task GetUserNotificationsAsync_WithValidUserId_ReturnsNotifications()
        {
            // Arrange
            var notifications = CreateTestNotifications();
            var mockQueryable = CreateMockQueryable(notifications);

            _mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.GetUserNotificationsAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Notifications.Count, Is.EqualTo(2)); // Only notifications for test user
            Assert.That(result.CurrentPage, Is.EqualTo(1));
            Assert.That(result.NotificationsPerPage, Is.EqualTo(10));
            Assert.That(result.TotalNotifications, Is.EqualTo(2));

            var firstNotification = result.Notifications.First();
            Assert.That(firstNotification.Message, Is.EqualTo("Test notification 2"));
            Assert.That(firstNotification.IsRead, Is.False);
        }

        [Test]
        public async Task GetUserNotificationsAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var notifications = CreateLargeTestNotificationSet();
            var mockQueryable = CreateMockQueryable(notifications);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.GetUserNotificationsAsync(_testUserId.ToString(), page: 2, pageSize: 5);

            // Assert
            Assert.That(result.CurrentPage, Is.EqualTo(2));
            Assert.That(result.NotificationsPerPage, Is.EqualTo(5));
            Assert.That(result.Notifications.Count, Is.EqualTo(5));
            Assert.That(result.TotalNotifications, Is.EqualTo(12));
        }

        [Test]
        public async Task GetUserNotificationsAsync_WithInvalidUserId_ReturnsEmptyViewModel()
        {
            // Arrange
            string invalidUserId = "invalid-guid";

            // Act
            var result = await this._notificationService.GetUserNotificationsAsync(invalidUserId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Notifications, Is.Not.Null);
            Assert.That(result.Notifications.Count, Is.EqualTo(0));
            Assert.That(result.CurrentPage, Is.EqualTo(1));
            Assert.That(result.NotificationsPerPage, Is.EqualTo(10)); 
            Assert.That(result.TotalNotifications, Is.EqualTo(0));
        }

        [Test]
        public async Task GetUserNotificationsAsync_WithNullUserId_ReturnsEmptyViewModel()
        {
            // Act
            var result = await this._notificationService.GetUserNotificationsAsync(null!);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Notifications.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task CreateAsync_WithValidParameters_CreatesNotification()
        {
            // Arrange
            string message = "Test notification message";
            Notification? capturedNotification = null;

            this._mockNotificationRepository
                .Setup(r => r.AddAsync(It.IsAny<Notification>()))
                .Callback<Notification>(n => capturedNotification = n)
                .Returns(Task.CompletedTask);

            // Act
            await this._notificationService.CreateAsync(_testUserId.ToString(), message);

            // Assert
            this._mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Once);
            Assert.That(capturedNotification, Is.Not.Null);
            Assert.That(capturedNotification.ApplicationUserId, Is.EqualTo(_testUserId));
            Assert.That(capturedNotification.Message, Is.EqualTo(message));
            Assert.That(capturedNotification.IsRead, Is.False);
        }

        [Test]
        public async Task CreateAsync_WithInvalidUserId_DoesNotCreateNotification()
        {
            // Arrange
            string invalidUserId = "invalid-guid";
            string message = "Test message";

            // Act
            await this._notificationService.CreateAsync(invalidUserId, message);

            // Assert
            this._mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Never);
        }

        [Test]
        public async Task CreateAsync_WithNullUserId_DoesNotCreateNotification()
        {
            // Act
            await _notificationService.CreateAsync(null!, "Test message");

            // Assert
            this._mockNotificationRepository.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Never);
        }

        [Test]
        public async Task MarkAsReadAsync_WithValidNotificationId_MarksAsReadAndReturnsTrue()
        {
            // Arrange
            var notification = new Notification
            {
                Id = _testNotificationId,
                ApplicationUserId = _testUserId,
                Message = "Test notification",
                CreatedOn = DateTime.UtcNow,
                IsRead = false
            };

            this._mockNotificationRepository
                .Setup(r => r.GetByIdAsync(_testNotificationId))
                .ReturnsAsync(notification);

            this._mockNotificationRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._notificationService.MarkAsReadAsync(_testNotificationId.ToString());

            // Assert
            Assert.That(result, Is.True);
            Assert.That(notification.IsRead, Is.True);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task MarkAsReadAsync_WithInvalidNotificationId_ReturnsFalse()
        {
            // Arrange
            string invalidId = "invalid-guid";

            // Act
            var result = await this._notificationService.MarkAsReadAsync(invalidId);

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task MarkAsReadAsync_WithNonExistentNotification_ReturnsFalse()
        {
            // Arrange
            this._mockNotificationRepository
                .Setup(r => r.GetByIdAsync(_testNotificationId))
                .ReturnsAsync((Notification)null!);

            // Act
            var result = await this._notificationService.MarkAsReadAsync(_testNotificationId.ToString());

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task MarkAllAsReadAsync_WithUnreadNotifications_MarksAllAsReadAndReturnsTrue()
        {
            // Arrange
            var unreadNotifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = "Unread 1",
                    CreatedOn = DateTime.UtcNow,
                    IsRead = false
                },
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = "Unread 2",
                    CreatedOn = DateTime.UtcNow,
                    IsRead = false
                }
            };

            var mockQueryable = CreateMockQueryable(unreadNotifications);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            this._mockNotificationRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._notificationService.MarkAllAsReadAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.True);
            Assert.That(unreadNotifications.All(n => n.IsRead), Is.True);
            _mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task MarkAllAsReadAsync_WithNoUnreadNotifications_ReturnsFalse()
        {
            // Arrange
            var emptyList = new List<Notification>();
            var mockQueryable = CreateMockQueryable(emptyList);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.MarkAllAsReadAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task MarkAllAsReadAsync_WithInvalidUserId_ReturnsFalse()
        {
            // Act
            var result = await this._notificationService.MarkAllAsReadAsync("invalid-guid");

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task DeleteNotificationAsync_WithValidNotificationId_DeletesAndReturnsTrue()
        {
            // Arrange
            var notification = new Notification
            {
                Id = _testNotificationId,
                ApplicationUserId = _testUserId,
                Message = "Test notification",
                CreatedOn = DateTime.UtcNow,
                IsRead = false
            };

            this._mockNotificationRepository
                .Setup(r => r.GetByIdAsync(_testNotificationId))
                .ReturnsAsync(notification);

            this._mockNotificationRepository
                .Setup(r => r.HardDelete(notification))
                .Verifiable();

            this._mockNotificationRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._notificationService.DeleteNotificationAsync(_testNotificationId.ToString());

            // Assert
            Assert.That(result, Is.True);
            this._mockNotificationRepository.Verify(r => r.HardDelete(notification), Times.Once);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteNotificationAsync_WithInvalidNotificationId_ReturnsFalse()
        {
            // Act
            var result = await this._notificationService.DeleteNotificationAsync("invalid-guid");

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            this._mockNotificationRepository.Verify(r => r.HardDelete(It.IsAny<Notification>()), Times.Never);
        }

        [Test]
        public async Task DeleteNotificationAsync_WithNonExistentNotification_ReturnsFalse()
        {
            // Arrange
            this._mockNotificationRepository
                .Setup(r => r.GetByIdAsync(_testNotificationId))
                .ReturnsAsync((Notification)null!);

            // Act
            var result = await this._notificationService.DeleteNotificationAsync(_testNotificationId.ToString());

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.HardDelete(It.IsAny<Notification>()), Times.Never);
            this._mockNotificationRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task GetUnreadCountAsync_WithValidUserId_ReturnsCorrectCount()
        {
            // Arrange
            var notifications = CreateTestNotifications();
            var mockQueryable = CreateMockQueryable(notifications);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.GetUnreadCountAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUnreadCountAsync_WithInvalidUserId_ReturnsZero()
        {
            // Act
            var result = await this._notificationService.GetUnreadCountAsync("invalid-guid");

            // Assert
            Assert.That(result, Is.EqualTo(0));
            this._mockNotificationRepository.Verify(r => r.GetAllAttached(), Times.Never);
        }

        [Test]
        public async Task HasUnreadNotificationsAsync_WithUnreadNotifications_ReturnsTrue()
        {
            // Arrange
            var notifications = CreateTestNotifications();
            var mockQueryable = CreateMockQueryable(notifications);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.HasUnreadNotificationsAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task HasUnreadNotificationsAsync_WithNoUnreadNotifications_ReturnsFalse()
        {
            // Arrange
            var readNotifications = new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = "Read notification",
                    CreatedOn = DateTime.UtcNow,
                    IsRead = true
                }
            };

            var mockQueryable = CreateMockQueryable(readNotifications);

            this._mockNotificationRepository
                .Setup(r => r.GetAllAttached())
                .Returns(mockQueryable.Object);

            // Act
            var result = await this._notificationService.HasUnreadNotificationsAsync(_testUserId.ToString());

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task HasUnreadNotificationsAsync_WithInvalidUserId_ReturnsFalse()
        {
            // Act
            var result = await _notificationService.HasUnreadNotificationsAsync("invalid-guid");

            // Assert
            Assert.That(result, Is.False);
            this._mockNotificationRepository.Verify(r => r.GetAllAttached(), Times.Never);
        }

        private List<Notification> CreateTestNotifications()
        {
            Guid otherUserId = Guid.NewGuid();

            return new List<Notification>
            {
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = "Test notification 1",
                    CreatedOn = DateTime.UtcNow.AddHours(-2),
                    IsRead = true
                },
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = "Test notification 2",
                    CreatedOn = DateTime.UtcNow.AddHours(-1),
                    IsRead = false
                },
                new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = otherUserId,
                    Message = "Other user notification",
                    CreatedOn = DateTime.UtcNow,
                    IsRead = false
                }
            };
        }

        private List<Notification> CreateLargeTestNotificationSet()
        {
            List<Notification> notifications = new List<Notification>();

            for (int i = 1; i <= 12; i++)
            {
                notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    ApplicationUserId = _testUserId,
                    Message = $"Test notification {i}",
                    CreatedOn = DateTime.UtcNow.AddHours(-i),
                    IsRead = i % 3 == 0
                });
            }

            return notifications.OrderByDescending(n => n.CreatedOn).ToList();
        }

        private Mock<IQueryable<Notification>> CreateMockQueryable(List<Notification> notifications)
        {
            var mockQueryable = new Mock<IQueryable<Notification>>();
            mockQueryable.As<IAsyncEnumerable<Notification>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Notification>(notifications.GetEnumerator()));

            mockQueryable.As<IQueryable<Notification>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Notification>(notifications.AsQueryable().Provider));

            mockQueryable.As<IQueryable<Notification>>().Setup(m => m.Expression).Returns(notifications.AsQueryable().Expression);
            mockQueryable.As<IQueryable<Notification>>().Setup(m => m.ElementType).Returns(notifications.AsQueryable().ElementType);
            mockQueryable.As<IQueryable<Notification>>().Setup(m => m.GetEnumerator()).Returns(notifications.GetEnumerator());

            return mockQueryable;
        }
    }
}