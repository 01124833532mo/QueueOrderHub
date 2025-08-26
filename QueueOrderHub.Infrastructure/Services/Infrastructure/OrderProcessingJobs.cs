using Hangfire;
using Microsoft.Extensions.Logging;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Messages;
using QueueOrderHub.Core.Domain.Models.Orders;

namespace QueueOrderHub.Infrastructure.Services.Infrastructure
{
    public class OrderProcessingJobs
    {

        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;
        public OrderProcessingJobs(ILogger<OrderProcessingJobs> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }
        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessOrderAsync(OrderMessage orderMessage)
        {

            var order = _orderRepository.GetOrderStatusAsync(orderMessage.OrderId);
            if (order is null)
            {
                var newOrder = new Order
                {
                    Id = orderMessage.OrderId,
                    CustomerName = orderMessage.CustomerName,
                    Product = orderMessage.Product,
                    Quantity = orderMessage.Quantity,
                    TotalAmount = orderMessage.TotalAmount,
                    Status = OrderStatus.Pending,
                    CreatedAt = orderMessage.CreatedAt
                };
                await _orderRepository.StoreOrderStatusAsync(newOrder);
            }

            await _orderRepository.UpdateOrderStatusAsync(new Order
            {
                Id = orderMessage.OrderId,
                CustomerName = orderMessage.CustomerName,
                Product = orderMessage.Product,
                Quantity = orderMessage.Quantity,
                TotalAmount = orderMessage.TotalAmount,
                Status = OrderStatus.Processing,
                CreatedAt = orderMessage.CreatedAt
            });





        }

    }
}
