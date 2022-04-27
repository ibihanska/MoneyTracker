using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Interfaces;

namespace WebJobs
{
    public class AccountService : IAccountService
    {
        private readonly IMoneyTrackerDbContext _context;
        private readonly IMapper _mapper;

        public AccountService(IMoneyTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<TransactionDto> GetReport(Guid accountId)
        {
            var transactionsQuery = _context.Transactions.AsQueryable().AsNoTracking();
            transactionsQuery = transactionsQuery.Where(x => x.ToAccount.Id == accountId || x.FromAccount.Id == accountId);
            var transactions = transactionsQuery.ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
              .ToList();
            return transactions;
        }
    }
}
