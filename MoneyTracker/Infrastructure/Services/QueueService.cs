using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MoneyTracker.Infrastructure.Services
{
    public class QueueService : IQueueService
    {
        private readonly IOptions<StorageConnectionOptions> _config;
        public QueueService(IOptions<StorageConnectionOptions> config)
        {
            _config = config;
        }
        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            var connectionString = _config.Value.StorageConnectionString;
            var queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            var accountId = JsonConvert.SerializeObject(message, Formatting.Indented);
            if (queueClient.Exists())
            {
                await queueClient.SendMessageAsync(accountId);
            }
        }
    }
}
