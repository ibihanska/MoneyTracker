using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Infrastructure;
using MoneyTracker.Persistence;
using WebJobs.Services;

namespace WebJobs // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();
            builder.UseEnvironment(EnvironmentName.Development);
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();

            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
                // If the key exists in settings, use it to enable Application Insights.
                string instrumentationKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    b.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
                }
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<ICurrentUserService, CurrentUserService>();
                services.AddPersistence(context.Configuration);
                services.AddInfrastructure();
                services.AddScoped<IExportDataService, ExportDataService>();
                services.AddScoped<Functions>();
            });
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
