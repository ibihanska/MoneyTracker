using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;
using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.Application.TransactionCommands
{
    public class UpdateTransactionCommand : IRequest, IHaveMessages
    {
        public Guid Id { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TagName { get; set; }
        public string? Note { get; set; }
        public List<AccountReportMessage>? AccountReportMessages { get; set; }

        public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
        {
            public UpdateTransactionCommandValidator()
            {
                RuleFor(m => m.Id).NotEmpty();
                RuleFor(m => m.Amount).NotEmpty().GreaterThan(0);
                RuleFor(m => m.FromAccountId).NotEqual(m => m.ToAccountId);
                RuleFor(m => m.TransactionDate).NotEmpty();
                RuleFor(m => m.TagName).NotEmpty().When(m => m.FromAccountId == null || m.ToAccountId == null);
            }
        }
        public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IQueueService _queueService;
            public UpdateTransactionCommandHandler(IMoneyTrackerDbContext context, IQueueService queueService)
            {
                _context = context;
                _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            }


            public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
            {
                var transaction = await _context.Transactions
                   .Include(x => x.FromAccount)
                   .Include(x => x.ToAccount)
                   .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (transaction == null)
                {
                    throw new NotFoundException(nameof(Transaction), request.Id);
                }

                transaction.Update(request.FromAccountId, request.ToAccountId,
               request.TagName, request.Amount, request.Note, request.TransactionDate);

                await _context.SaveChangesAsync(cancellationToken);
                var accountReportMessages = new List<AccountReportMessage>();
                if (transaction.FromAccountId != null)
                {
                    accountReportMessages.Add(new AccountReportMessage { AccountId = (Guid)request.FromAccountId });
                }
                if (transaction.ToAccountId != null)
                {
                    accountReportMessages.Add(new AccountReportMessage { AccountId = (Guid)request.ToAccountId });
                }
                request.AccountReportMessages = accountReportMessages;
                return Unit.Value;
            }
        }
    }
}
