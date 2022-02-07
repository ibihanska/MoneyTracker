using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.AccountQueries
{
    public class GetAccountQuery : IRequest<AccountDto>
    {
        public Guid Id { get; set; }

        public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountDto>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IMapper _mapper;

            public GetAccountQueryHandler(IMoneyTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
            {
                var account = _context.Accounts.AsNoTracking()
                        .FirstOrDefault(c => c.Id == request.Id);
                if (account == null)
                {
                    throw new NotFoundException(nameof(Account), request.Id);
                }
                var accountDto = _mapper.Map<AccountDto>(account);
                return accountDto;
            }
        }
    }
}
