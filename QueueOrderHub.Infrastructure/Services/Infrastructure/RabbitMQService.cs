using Microsoft.Extensions.Configuration;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace QueueOrderHub.Infrastructure.Services.Infrastructure
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName = "orders";

        public RabbitMQService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void PublishOrder(OrderMessage orderMessage)
        {
            var message = JsonSerializer.Serialize(orderMessage);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _queueName,
                basicProperties: properties,
                body: body);
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
