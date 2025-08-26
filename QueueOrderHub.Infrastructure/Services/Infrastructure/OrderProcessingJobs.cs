using Hangfire;
using Microsoft.Extensions.Logging;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
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
        public async Task ProcessOrderAsync(Order order)
        {
            order.Status = OrderStatus.Processing;
            await _orderRepository.UpdateOrderStatusAsync(order);

        }

    }
}
