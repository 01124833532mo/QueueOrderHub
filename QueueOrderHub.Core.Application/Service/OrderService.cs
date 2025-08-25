using QueueOrderHub.Core.Application.Abstraction.Service;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Errors.Models;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Core.Application.Service
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        public async Task<OrderStatusResponse> CreateOrderAsync(Order Request)
        {
            if (Request is null)
            {
                throw new NotFoundExeption("Order Not Found ", Request.Id);
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
                throw new BadRequestExeption("Order Id Empty");
            }
            var orderStatus = await orderRepository.GetOrderStatusAsync(orderId);
            if (orderStatus is null)
            {
                throw new NotFoundExeption("Order Not Found with: ", orderId);
            }
            return orderStatus;
        }
    }
}
