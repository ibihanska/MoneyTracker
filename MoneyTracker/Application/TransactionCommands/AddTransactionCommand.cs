using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.TransactionCommands
{
    public class AddTransactionCommand : IRequest
    {
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Ammount { get; set; }
        public DateTime DateTime { get; set; }
        public Tag Tag { get; set; }
        public string? Note { get; set; }
        public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
        {
            public AddTransactionCommandValidator()
            {
                RuleFor(m => m.Ammount).NotEmpty().MustAsync(BeValidIdAsync).WithMessage("Amount must be greater than 0, it is {PropertyValue}");
                RuleFor(m => m.FromAccountId).NotEqual(m => m.ToAccountId);
                RuleFor(m => m.TransactionType).IsInEnum().NotEmpty();
                RuleFor(m => m.DateTime).NotEmpty();

            }
            private Task<bool> BeValidIdAsync(decimal amount, CancellationToken cancellationToken)
            {
                return Task.FromResult(amount > 0);
            }
        }
        public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand>
        {
            private readonly IMoneyTrackerDbContext _context;

            public AddTransactionCommandHandler(IMoneyTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
            {
                var transaction = new Transaction(
                    request.FromAccountId,
                    request.ToAccountId,
                    request.Tag,
                    request.TransactionType,
                    request.Ammount,
                    request.Note,
                    request.DateTime);

                var fromAccount = await _context.Accounts
                    .Include(x => x.ExpenseTransactions)
                    .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);
                fromAccount?.AddExpenseTransaction(transaction);

                var toAccount = await _context.Accounts
                    .Include(x => x.IncomeTransactions)
                    .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

                toAccount?.AddIncomeTransaction(transaction);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}