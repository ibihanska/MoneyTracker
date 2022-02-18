using FluentValidation;
using MediatR;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.AccountCommands
{
    public class EditAccountCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AccountType AccountType { get; set; }
        public class EditAccountCommandValidator : AbstractValidator<EditAccountCommand>
        {
            public EditAccountCommandValidator()
            {
                RuleFor(c => c.Name).NotEmpty();
                RuleFor(c => c.AccountType).IsInEnum();
                RuleFor(c => c.Id).NotEmpty();

            }
        }
        public class EditAccountCommandHandler : IRequestHandler<EditAccountCommand>
        {
            private readonly IMoneyTrackerDbContext _context;

            public EditAccountCommandHandler(IMoneyTrackerDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(EditAccountCommand request, CancellationToken cancellationToken)
            {
                var account = await _context.Accounts
                     .FindAsync(request.Id);
                if (account == null)
                {
                    throw new NotFoundException(nameof(Account), request.Id);
                }
                account.Update(request.Name, request.AccountType);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
