using QueueOrderHub.Core.Domain.Models.Messages;

namespace QueueOrderHub.Core.Domain.Contracts.Infrastructure
{
    public interface IRabbitMQService
    {
        void PublishOrder(OrderMessage orderMessage);

    }
}
