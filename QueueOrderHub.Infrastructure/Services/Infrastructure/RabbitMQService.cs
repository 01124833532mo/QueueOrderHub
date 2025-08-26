using Hangfire;
using Microsoft.Extensions.Configuration;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Core.Domain.Models.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

            StartConsuming();
        }

        public void PublishOrder(OrderMessage orderMessage)
        {
            var message = JsonSerializer.Serialize(orderMessage);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: string.Empty, // defualt Exchange
                routingKey: _queueName,
                basicProperties: properties,
                body: body);
        }
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }

        private void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var orderMessage = JsonSerializer.Deserialize<OrderMessage>(message);

                    if (orderMessage is not null)
                    {
                        BackgroundJob.Enqueue<OrderProcessingJobs>(x =>
                            x.ProcessOrderAsync(orderMessage));
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);
        }
    }
}
