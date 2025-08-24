using Microsoft.AspNetCore.Mvc;
using QueueOrderHub.Core.Application.Abstraction.Service;
using QueueOrderHub.Core.Domain.Models.Orders;

namespace QueueOrderHub.Apis.Controllers.Controllers.Orders
{
    public class OrderController(IOrderService orderService) : BaseApiController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null.");
            }
            var response = await orderService.CreateOrderAsync(order);
            return Ok(response);
        }
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetOrderStatusAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                return BadRequest("Invalid order ID.");
            }
            var response = await orderService.GetOrderStatusAsync(orderId);
            if (response == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(response);
        }
    }
}
