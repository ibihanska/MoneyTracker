using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.Application.TransactionCommands
{
    public class DeleteTransactionCommand : IRequest, IHaveMessages
    {
        public Guid Id { get; set; }
        public List<AccountReportMessage>? AccountReportMessages { get; set; }

        public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IQueueService _queueService;
            public DeleteTransactionCommandHandler(IMoneyTrackerDbContext context, IQueueService queueService)
            {
                _context = context;
                _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            }

            public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
            {
                var accountReportMessages = new List<AccountReportMessage>();
                var transaction = await _context.Transactions
                    .Include(x => x.FromAccount)
                    .Include(x => x.ToAccount)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                transaction?.FromAccount?.RemoveExpenseTransaction(transaction);
                transaction?.ToAccount?.RemoveIncomeTransaction(transaction);
                if (transaction.FromAccountId != null)
                {
                    accountReportMessages.Add(new AccountReportMessage { AccountId = (Guid)transaction.FromAccountId });
                }
                if (transaction.ToAccountId != null)
                {
                    accountReportMessages.Add(new AccountReportMessage { AccountId = (Guid)transaction.ToAccountId });
                }
                request.AccountReportMessages = accountReportMessages;
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
