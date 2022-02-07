using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.Common.Interfaces
{
    public interface IMoneyTrackerDbContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
