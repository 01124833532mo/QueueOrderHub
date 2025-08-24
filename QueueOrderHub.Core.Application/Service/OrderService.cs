using QueueOrderHub.Core.Application.Abstraction.Service;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Core.Application.Service
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<OrderStatusResponse> CreateOrderAsync(Order Request)
        {
            if (Request == null)
            {
                throw new ArgumentNullException(nameof(Request), "Order request cannot be null.");
            }

            await orderRepository.StoreOrderStatusAsync(Request);
            return new OrderStatusResponse
            {
                OrderId = Request.Id,
                Status = Request.Status,
                CustomerName = Request.CustomerName,
                Product = Request.Product,
                Quantity = Request.Quantity,
                TotalAmount = Request.TotalAmount
            };

        }

        public async Task<OrderStatusResponse?> GetOrderStatusAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
            }
            var orderStatus = await orderRepository.GetOrderStatusAsync(orderId);
            if (orderStatus == null)
            {
                return null;
            }
            return orderStatus;
        }
    }
}
