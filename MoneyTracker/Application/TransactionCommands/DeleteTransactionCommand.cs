using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;

namespace MoneyTracker.Application.TransactionCommands
{
    public class DeleteTransactionCommand : IRequest
    {
        public Guid Id { get; set; }
        public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
        {
            private readonly IMoneyTrackerDbContext _context;
            public DeleteTransactionCommandHandler(IMoneyTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
            {
                var transaction = await _context.Transactions
                    .Include(x => x.FromAccount)
                    .Include(x => x.ToAccount)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                transaction?.FromAccount?.RemoveExpenseTransaction(transaction);
                transaction?.ToAccount?.RemoveIncomeTransaction(transaction);

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
