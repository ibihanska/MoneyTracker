using FluentValidation;
using MediatR;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.AccountCommands
{
    public class AddAccountCommand : IRequest
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }

        public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
        {
            public AddAccountCommandValidator()
            {
                RuleFor(c => c.Name).NotEmpty();
                RuleFor(c => c.AccountType).IsInEnum();
                RuleFor(c => c.Balance).NotEmpty().GreaterThan(0);

            }
        }
        public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly ICurrentUserService _currentUser;
            public AddAccountCommandHandler(IMoneyTrackerDbContext context, ICurrentUserService currentUser)
            {
                _context = context;
                _currentUser = currentUser;
            }

            public async Task<Unit> Handle(AddAccountCommand request, CancellationToken cancellationToken)
            {
                var account = new Account(request.Name, request.Balance, request.AccountType, _currentUser.UserEmail);
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
