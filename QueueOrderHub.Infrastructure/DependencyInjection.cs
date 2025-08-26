using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueOrderHub.Core.Domain.Contracts.Infrastructure;
using QueueOrderHub.Infrastructure.Services.Infrastructure;
using QueueOrderHub.Infrastructure.Services.Orders;
using QueueOrderHub.Shared.ModelSettings;
using StackExchange.Redis;
namespace QueueOrderHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RedisSettings>(configuration.GetSection("RedisSetting"));


            services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            {
                var connectionString = configuration.GetConnectionString("Redis");

                var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);

                return connectionMultiplexerObj;

            });


            services.AddSingleton(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddSingleton(typeof(IRabbitMQService), typeof(RabbitMQService));

            return services;

        }
    }
}
