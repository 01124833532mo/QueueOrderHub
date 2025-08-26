using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace QueueOrderHub.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependency(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(h => h.UseSqlServerStorage(configuration.GetConnectionString("Hangfire")))
                 .AddHangfireServer();
            return services;
        }
    }
}
