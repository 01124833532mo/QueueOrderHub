using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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




            return services;

        }
    }
}
