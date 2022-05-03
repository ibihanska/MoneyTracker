using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IQueueService, QueueService>();
            services.AddTransient<IDateTime, MachineDateTime>();
            return services;
        }
    }
}
