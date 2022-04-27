using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(string queueName, T message);
    }
}
