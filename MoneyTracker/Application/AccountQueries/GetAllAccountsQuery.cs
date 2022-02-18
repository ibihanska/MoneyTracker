using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;

namespace MoneyTracker.Application.AccountQueries
{
    public class GetAllAccountsQuery : IRequest<List<AccountDto>>
    {

        public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, List<AccountDto>>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUser;

            public GetAllAccountsQueryHandler(IMoneyTrackerDbContext context, IMapper mapper, ICurrentUserService currentUser)
            {
                _context = context;
                _mapper = mapper;
                _currentUser = currentUser;
            }

            public async Task<List<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
            {
                var accounts = await _context.Accounts.Where(a => a.UserEmail == _currentUser.UserEmail).AsNoTracking()
                    .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return accounts;
            }
        }
    }
}
