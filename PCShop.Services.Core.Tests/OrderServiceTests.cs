using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Moq;
using PCShop.Data.Models;
using PCShop.Data.Models.Enum;
using PCShop.Data.Repository.Interfaces;
using PCShop.Services.Core.Interfaces;
using PCShop.Web.ViewModels.Order;
using static PCShop.Services.Common.ServiceConstants;

namespace PCShop.Services.Core.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepo;
        private Mock<IOrderItemRepository> _mockOrderItemRepo;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IEmailSender> _mockEmailSender;
        private Mock<INotificationService> _mockNotificationService;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            this._mockOrderRepo = new Mock<IOrderRepository>();
            this._mockOrderItemRepo = new Mock<IOrderItemRepository>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            this._mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            this._mockEmailSender = new Mock<IEmailSender>();
            this._mockNotificationService = new Mock<INotificationService>();

            this._orderService = new OrderService(
                this._mockOrderRepo.Object,
                this._mockOrderItemRepo.Object,
                this._mockUserManager.Object,
                this._mockEmailSender.Object,
                this._mockNotificationService.Object);
        }

        private ApplicationUser CreateTestUser(string userId = null!, string email = "test@example.com", string fullName = "Test User")
        {
            return new ApplicationUser
            {
                Id = userId != null ? Guid.Parse(userId) : Guid.NewGuid(),
                UserName = email,
                Email = email,
                City = "Sofia",
                Address = "Test Address",
                PostalCode = "1000",
                FullName = fullName,
                PhoneNumber = "1234567890"
            };
        }

        private Product CreateTestProduct(Guid? id = null, string name = "Test Product", decimal price = 1000)
        {
            return new Product
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                Description = "Test Description",
                Price = price,
                ImageUrl = "test.jpg",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        [Test]
        public async Task AddProductToCartAsync_NewOrder_CreatesOrderAndItem()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var model = new AddToCartFormModel
            {
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 2
            };

            this._mockOrderRepo.Setup(r => r.GetPendingOrderWithItemsAsync(userId))
                .ReturnsAsync((Order)null!);

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(CreateTestUser(userId));

            // Act
            var result = await this._orderService.AddProductToCartAsync(model, userId);

            // Assert
            Assert.IsTrue(result);
            this._mockOrderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            this._mockOrderItemRepo.Verify(r => r.AddAsync(It.IsAny<OrderItem>()), Times.Once);
        }

        [Test]
        public async Task GetCartItemsAsync_WithItems_ReturnsCorrectViewModels()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var product = CreateTestProduct();

            var order = new Order
            {
                OrdersItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        Product = product,
                        Quantity = 2
                    }
                }
            };

            this._mockOrderRepo.Setup(r => r.GetPendingOrderWithItemsAsync(userId))
                .ReturnsAsync(order);

            // Act
            var result = (await _orderService.GetCartItemsAsync(userId)).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(product.Name));
            Assert.That(result[0].Price, Is.EqualTo(product.Price));
            Assert.That(result[0].Quantity, Is.EqualTo(2));
        }

        [Test]
        public async Task FinalizeOrderWithDetailsAsync_ValidOrder_UpdatesOrderStatus()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser(userId);
            var product = CreateTestProduct();

            var order = new Order
            {
                OrdersItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Quantity = 1,
                        Product = product
                    }
                }
            };

            var model = new OrderConfirmationInputModel
            {
                DeliveryMethod = DeliveryMethod.Econt,
                PaymentMethod = OrderPaymentMethod.Card,
                City = "Sofia",
                PostalCode = "1000",
                Address = "Test Address"
            };

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(user);

            this._mockOrderRepo.Setup(r => r.GetPendingOrderWithItemsAsync(userId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.FinalizeOrderWithDetailsAsync(userId, model);

            // Assert
            Assert.IsTrue(result);
            this._mockOrderRepo.Verify(r => r.UpdateAsync(It.Is<Order>(o =>
                o.Status == OrderStatus.Completed && o.TotalPrice == product.Price + EcontFee)), Times.Once);
        }

        [Test]
        public async Task CalculateDeliveryFee_ReturnsCorrectFees()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = CreateTestUser(userId);
            var product = CreateTestProduct(price: 1000);

            Order CreateTestOrder() => new Order
            {
                OrdersItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 1, Product = product }
                }
            };

            this._mockUserManager.Setup(m => m.FindByIdAsync(userId))
                .ReturnsAsync(user);

            var econtOrder = CreateTestOrder();
            this._mockOrderRepo.Setup(r => r.GetPendingOrderWithItemsAsync(userId))
                .ReturnsAsync(econtOrder);

            var econtModel = new OrderConfirmationInputModel
            {
                DeliveryMethod = DeliveryMethod.Econt,
                City = "Test City",
                PostalCode = "1000",
                Address = "Test Address",
                PaymentMethod = OrderPaymentMethod.Card
            };

            // Act & Assert Econt
            await this._orderService.FinalizeOrderWithDetailsAsync(userId, econtModel);
            Assert.That(econtOrder.DeliveryFee, Is.EqualTo(EcontFee));

            var speedyOrder = CreateTestOrder();
            this._mockOrderRepo.Setup(r => r.GetPendingOrderWithItemsAsync(userId))
                .ReturnsAsync(speedyOrder);

            var speedyModel = new OrderConfirmationInputModel
            {
                DeliveryMethod = DeliveryMethod.Speedy,
                City = "Test City",
                PostalCode = "1000",
                Address = "Test Address",
                PaymentMethod = OrderPaymentMethod.Card
            };

            // Act & Assert Speedy
            await this._orderService.FinalizeOrderWithDetailsAsync(userId, speedyModel);
            Assert.That(speedyOrder.DeliveryFee, Is.EqualTo(SpeedyFee));
        }
    }
}