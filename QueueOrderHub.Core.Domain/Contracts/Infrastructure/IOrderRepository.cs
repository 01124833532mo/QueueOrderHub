using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Core.Domain.Contracts.Infrastructure
{
    public interface IOrderRepository
    {

        Task StoreOrderStatusAsync(Order order);
        Task<OrderStatusResponse?> GetOrderStatusAsync(Guid orderId);
    }
}
