using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.TransactionQueries
{
    public class GetAllFilterTransactionsQuery : IRequest<List<TransactionDto>>
    {
        public string TagName { get; set; }
        public DateTime Date { get; set; }
        public string AccountName { get; set; }

        public class GetAllFilterTransactionsQueryHandler : IRequestHandler<GetAllFilterTransactionsQuery, List<TransactionDto>>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly ICurrentUserService _currentUser;
            private readonly IMapper _mapper;

            public GetAllFilterTransactionsQueryHandler(IMoneyTrackerDbContext context, ICurrentUserService currentUser, IMapper mapper)
            {
                _context = context;
                _currentUser = currentUser;
                _mapper = mapper;
            }

            public async Task<List<TransactionDto>> Handle(GetAllFilterTransactionsQuery request, CancellationToken cancellationToken)
            {
                var transactionsQuery = _context.Transactions.AsQueryable().AsNoTracking();
                var accountName = request.AccountName?.Trim();
                if (!string.IsNullOrEmpty(accountName))
                {
                    transactionsQuery = transactionsQuery.Where(x =>
                        (x.FromAccount != null && x.FromAccount.Name == accountName) ||
                        (x.ToAccount != null && x.ToAccount.Name == accountName));
                }
                var tagNam = request.TagName?.Trim();
                if (!string.IsNullOrEmpty(tagNam))
                {
                    transactionsQuery = transactionsQuery.Where(x => x.Tag.Name == tagNam);
                }
                if(request.Date!=null)
                {
                    transactionsQuery = transactionsQuery.Where(x => x.TransactionDate>=request.Date);
                }
                var transactions = transactionsQuery.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);
              
                return await transactions;
            }
        }
    }
}
