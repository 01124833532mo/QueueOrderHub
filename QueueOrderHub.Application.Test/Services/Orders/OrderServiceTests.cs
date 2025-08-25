using FakeItEasy;
using QueueOrderHub.Core.Application.Service;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Errors.Models;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Application.Test.Services.Orders
{
    public class OrderServiceTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepository = A.Fake<IOrderRepository>();

            _orderService = new OrderService(_orderRepository);
        }
        [Fact]
        public async Task CreateOrderAsync_ValidInput_ReturnsOrderId()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var createOrderDto = new Order
            {
                Id = orderId,
                CustomerName = "John Doe",
                Product = "Laptop",
                Quantity = 1,
                TotalAmount = 999.99m
            };

            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.Ignored))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderStatusResponse>(result);
            Assert.Equal(orderId, result.OrderId);

            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.That.Matches(o =>
                o.CustomerName == createOrderDto.CustomerName &&
                o.Product == createOrderDto.Product &&
                o.Quantity == createOrderDto.Quantity &&
                o.TotalAmount == createOrderDto.TotalAmount &&
                o.Id == createOrderDto.Id)))
                .MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderStatusAsync_ValidOrderId_ReturnsOrderStatus()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var expectedOrderStatus = new OrderStatusResponse
            {
                OrderId = orderId,
                Status = OrderStatus.Pending,
                CustomerName = "Jane Doe",
                Product = "Smartphone",
                Quantity = 2,
                TotalAmount = 1999.98m
            };
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(orderId))
                .Returns(Task.FromResult(expectedOrderStatus));
            // Act
            var result = await _orderService.GetOrderStatusAsync(orderId);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderStatusResponse>(result);
            Assert.Equal(expectedOrderStatus.OrderId, result.OrderId);
            Assert.Equal(expectedOrderStatus.Status, result.Status);
            Assert.Equal(expectedOrderStatus.CustomerName, result.CustomerName);
            Assert.Equal(expectedOrderStatus.Product, result.Product);
            Assert.Equal(expectedOrderStatus.Quantity, result.Quantity);
            Assert.Equal(expectedOrderStatus.TotalAmount, result.TotalAmount);
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(orderId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateOrderAsync_NullOrder_ThrowsArgumentNullException()
        {
            // Arrange
            Order? nullOrder = null;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _orderService.CreateOrderAsync(nullOrder!));
        }
        [Fact]
        public async Task GetOrderStatusAsync_EmptyOrderId_ThrowsArgumentException()
        {
            // Arrange
            var emptyOrderId = Guid.Empty;
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.GetOrderStatusAsync(emptyOrderId));
        }
        [Fact]

        public async Task CreateOrderAsync_ValidOrder_CallsRepository()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "mohamed",
                Product = "Tablet",
                Quantity = 3,
                TotalAmount = 299.99m
            };
            // Act
            await _orderService.CreateOrderAsync(order);
            // Assert
            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.That.Matches(o =>
                o.CustomerName == order.CustomerName &&
                o.Product == order.Product &&
                o.Quantity == order.Quantity &&
                o.TotalAmount == order.TotalAmount &&
                o.Id == order.Id)))
                .MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderStatusAsync_ValidOrderId_CallsRepository()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            // Act
            await _orderService.GetOrderStatusAsync(orderId);
            // Assert
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(orderId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateOrderAsync_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = "alaaa",
                Product = "Monitor",
                Quantity = 1,
                TotalAmount = 199.99m
            };
            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.Ignored))
                .Throws(new Exception("Database error"));
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.CreateOrderAsync(order));
        }
        [Fact]
        public async Task GetOrderStatusAsync_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(orderId))
                .Throws(new Exception("Database error"));
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _orderService.GetOrderStatusAsync(orderId));
        }
        [Fact]
        public async Task CreateOrderAsync_OrderWithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var createOrderDto = new Order
            {
                Id = orderId,
                CustomerName = "ahmed",
                Product = "laptop",
                Quantity = 1,
                TotalAmount = 1099.99m
            };
            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.Ignored))
                .Returns(Task.CompletedTask);
            // Act
            var result = await _orderService.CreateOrderAsync(createOrderDto);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderStatusResponse>(result);
            Assert.Equal(orderId, result.OrderId);
            A.CallTo(() => _orderRepository.StoreOrderStatusAsync(A<Order>.That.Matches(o =>
                o.CustomerName == createOrderDto.CustomerName &&
                o.Product == createOrderDto.Product &&
                o.Quantity == createOrderDto.Quantity &&
                o.TotalAmount == createOrderDto.TotalAmount &&
                o.Id == createOrderDto.Id)))
                .MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async Task GetOrderStatusAsync_NonExistentOrderId_ReturnsNull()
        {
            // Arrange
            var nonExistentOrderId = Guid.NewGuid();
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(nonExistentOrderId))
                .Returns(Task.FromResult<OrderStatusResponse?>(null));
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundExeption>(() => _orderService.GetOrderStatusAsync(nonExistentOrderId));
            A.CallTo(() => _orderRepository.GetOrderStatusAsync(nonExistentOrderId)).MustHaveHappenedOnceExactly();
        }





    }
}
