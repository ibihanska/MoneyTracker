using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure;

namespace MoneyTracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IQueueService, QueueService>();
            return services;
        }
    }
}
