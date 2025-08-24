using Microsoft.Extensions.DependencyInjection;
using QueueOrderHub.Core.Application.Abstraction.Service;
using QueueOrderHub.Core.Application.Service;

namespace QueueOrderHub.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IOrderService), typeof(OrderService));


            return services;
        }

    }
}
