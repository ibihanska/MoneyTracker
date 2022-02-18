using MediatR;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.AccountCommands
{
    public class DeleteAccountCommand : IRequest
    {
        public Guid Id { get; set; }
        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteAccountCommand>
        {
            private readonly IMoneyTrackerDbContext _context;

            public DeleteCategoryCommandHandler(IMoneyTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
            {
                var account = await _context.Accounts
                    .FindAsync(request.Id);
                _context.Accounts.Remove(account);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
