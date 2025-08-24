namespace QueueOrderHub.Core.Domain.Models.Messages
{
    public class OrderMessage
    {

        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
