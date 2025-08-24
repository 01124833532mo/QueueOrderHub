using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Core.Application.Abstraction.Service
{
    public interface IOrderService
    {
        Task<OrderStatusResponse> CreateOrderAsync(Order Request);
        Task<OrderStatusResponse?> GetOrderStatusAsync(Guid orderId);
    }
}
