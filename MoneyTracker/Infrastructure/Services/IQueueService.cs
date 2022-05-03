namespace MoneyTracker.Infrastructure.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(string queueName, T message);
    }
}
