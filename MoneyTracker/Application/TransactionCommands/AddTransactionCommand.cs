﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;
using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.Application.TransactionCommands
{
    public class AddTransactionCommand : IRequest<Guid>, IHaveMessages
    {
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
        public List<AccountReportMessage>? AccountReportMessages { get; set; }

        public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
        {
            public AddTransactionCommandValidator()
            {
                RuleFor(m => m.Amount).NotEmpty().GreaterThan(0);
                RuleFor(m => m.FromAccountId).NotEqual(m => m.ToAccountId);
                RuleFor(m => m.TransactionDate).NotEmpty();
                RuleFor(m => m.TagName).NotEmpty().When(m => m.FromAccountId == null || m.ToAccountId == null);
            }
        }

        public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, Guid>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IQueueService _queueService;

            public AddTransactionCommandHandler(IMoneyTrackerDbContext context, IQueueService queueService)
            {
                _context = context;
                _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            }

            public async Task<Guid> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
            {
                var transaction = new Transaction(
                    request.FromAccountId,
                    request.ToAccountId,
                    request.TagName,
                    request.Amount,
                    request.Note,
                    request.TransactionDate);

                var fromAccount = await _context.Accounts
                    .Include(x => x.ExpenseTransactions)
                    .FirstOrDefaultAsync(x => x.Id == request.FromAccountId, cancellationToken);
                fromAccount?.AddExpenseTransaction(transaction);

                var toAccount = await _context.Accounts
                    .Include(x => x.IncomeTransactions)
                    .FirstOrDefaultAsync(x => x.Id == request.ToAccountId, cancellationToken);

                toAccount?.AddIncomeTransaction(transaction);

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
                return transaction.Id;
            }
        }
    }
}
