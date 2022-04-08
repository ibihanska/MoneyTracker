using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;

namespace MoneyTracker.Application.AccountQueries
{
    public class GetAccountInfoQuery : IRequest<List<AccountInfoDto>>
    {

        public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, List<AccountInfoDto>>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUser;

            public GetAccountInfoQueryHandler(IMoneyTrackerDbContext context, IMapper mapper, ICurrentUserService currentUser)
            {
                _context = context;
                _mapper = mapper;
                _currentUser = currentUser;
            }

            public async Task<List<AccountInfoDto>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
            {
                var accounts = await _context.Accounts.Where(a => a.UserEmail == _currentUser.UserEmail).AsNoTracking()
                    .ProjectTo<AccountInfoDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return accounts;
            }
        }
    }
}
