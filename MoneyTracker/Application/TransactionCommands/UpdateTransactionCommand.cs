using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.TransactionCommands
{
    public class UpdateTransactionCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public TransactionType TransactionType => FromAccountId.HasValue && ToAccountId.HasValue ? TransactionType.Transfer :
           FromAccountId.HasValue ? TransactionType.Expense : TransactionType.Income;
        public decimal Ammount { get; set; }
        public DateTime DateTime { get; set; }
        public Tag Tag { get; set; }
        public string? Note { get; set; }
        public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
        {
            public UpdateTransactionCommandValidator()
            {
                RuleFor(m => m.Id).NotEmpty();
                RuleFor(m => m.Ammount).NotEmpty().GreaterThan(0);
                RuleFor(m => m.FromAccountId).NotEqual(m => m.ToAccountId);
                RuleFor(m => m.DateTime).NotEmpty();
            }
        }
        public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
        {
            private readonly IMoneyTrackerDbContext _context;
            public UpdateTransactionCommandHandler(IMoneyTrackerDbContext context)
            {
                _context = context;
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
                if (transaction.TransactionType != request.TransactionType)
                {
                    transaction.Update(request.FromAccountId, request.ToAccountId,
                   request.Tag, request.TransactionType, request.Ammount, request.Note, request.DateTime);
                }
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
