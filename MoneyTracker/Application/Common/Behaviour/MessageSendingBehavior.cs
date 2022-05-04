using MediatR;
using MoneyTracker.Application.TransactionCommands;
using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.Application.Common.Behaviour
{
    public class MessageSendingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IHaveMessages
    {
        private readonly IQueueService _queueService;
        private TResponse result;

        public MessageSendingBehavior(IQueueService queueService)
        {
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await next();
            foreach (var message in request.AccountReportMessages)
            {
                await _queueService.SendMessageAsync(AccountReportMessage.QueueName, new AccountReportMessage { AccountId = message.AccountId });
            }
            return result;
        }
    }
}
