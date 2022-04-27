using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace MoneyTracker.Infrastructure
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;
        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            string connectionString = _configuration["StorageConnectionString"];
            var queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            string accountId = JsonConvert.SerializeObject(message, Formatting.Indented);
            if (queueClient.Exists())
            {
                await queueClient.SendMessageAsync(accountId);
            }
        }
    }
}
