using QueueOrderHub.Core.Application.Abstraction.Service;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Messages;
using QueueOrderHub.Core.Domain.Models.Orders;
using QueueOrderHub.Shared.Errors.Models;
using QueueOrderHub.Shared.Response.Order;

namespace QueueOrderHub.Core.Application.Service
{
    public class OrderService(IOrderRepository orderRepository, IRabbitMQService rabbitMQService) : IOrderService
    {
        public async Task<OrderStatusResponse> CreateOrderAsync(Order Request)
        {
            if (Request is null)
            {
                throw new NotFoundExeption("Order Not Found ", Request!.Id);
            }

            await orderRepository.StoreOrderStatusAsync(Request);

            // Publish to RabbitMQ
            var orderMessage = new OrderMessage
            {
                OrderId = Request.Id,
                CustomerName = Request.CustomerName,
                Product = Request.Product,
                Quantity = Request.Quantity,
                TotalAmount = Request.TotalAmount,
                CreatedAt = Request.CreatedAt
            };
            try
            {

                rabbitMQService.PublishOrder(orderMessage);

            }
            catch (Exception ex)
            {
                throw new BadRequestExeption($"RabbitMq Faild To Push Message And Details :{ex.Message} ");
            }



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
