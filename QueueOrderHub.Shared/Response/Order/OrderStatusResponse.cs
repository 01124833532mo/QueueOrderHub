using QueueOrderHub.Core.Domain.Models.Orders;

namespace QueueOrderHub.Shared.Response.Order
{
    public class OrderStatusResponse
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }



    }
}
