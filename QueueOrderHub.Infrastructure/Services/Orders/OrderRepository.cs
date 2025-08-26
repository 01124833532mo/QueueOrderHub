using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Shared.Response.Order;
using StackExchange.Redis;
using System.Text.Json;

namespace QueueOrderHub.Infrastructure.Services.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDatabase _redisDb;

        public OrderRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }


        public async Task StoreOrderStatusAsync(Core.Domain.Models.Orders.Order order)
        {
            var statusResponse = new OrderStatusResponse
            {
                OrderId = order.Id,
                Status = order.Status,
                CustomerName = order.CustomerName,
                Product = order.Product,
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount


            };

            var serializedStatus = JsonSerializer.Serialize(statusResponse);
            await _redisDb.StringSetAsync($"order:{order.Id}", serializedStatus, TimeSpan.FromHours(24));
        }

        public async Task<OrderStatusResponse?> GetOrderStatusAsync(Guid orderId)
        {
            var serializedStatus = await _redisDb.StringGetAsync($"order:{orderId}");
            if (serializedStatus.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<OrderStatusResponse>(serializedStatus!);
        }
    }
}
