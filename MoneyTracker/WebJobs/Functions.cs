using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MoneyTracker.Application.Common;
using WebJobs.Services;

namespace WebJobs
{
    public class Functions
    {
        private readonly IExportDataService _exportData;

        public Functions(IExportDataService exportData)
        {
            _exportData = exportData;
        }

        public async Task ProcessQueueMessageAsync(
              [QueueTrigger(AccountReportMessage.QueueName)] AccountReportMessage request, ILogger logger)
        {
            await _exportData.ExportCsvAsync(request.AccountId);
            logger.LogInformation("Report generated!");
        }
    }
}
