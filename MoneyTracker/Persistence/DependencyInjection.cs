using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Application.Common.Interfaces;

namespace MoneyTracker.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MoneyTrackerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MoneyTrackerDatabase")));

            services.AddScoped<IMoneyTrackerDbContext>(provider => provider.GetService<MoneyTrackerDbContext>());

            return services;
        }
    }
}
