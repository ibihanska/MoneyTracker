using AutoMapper;
using MoneyTracker.Application.Common.Mapping;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.AccountQueries
{
    public class AccountDto : IMapFrom<Account>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
    }
}
