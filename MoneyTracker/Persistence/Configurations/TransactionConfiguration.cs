using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.TransactionDate).HasColumnType("datetime").IsRequired();
            builder.OwnsOne(x => x.Tag).Property(e => e.Name).HasColumnName("TagName");
            builder.Property(x => x.Amount).HasColumnType("money").IsRequired();
            builder.HasOne(x => x.ToAccount).WithMany(x => x.IncomeTransactions).HasForeignKey(x => x.ToAccountId);
            builder.HasOne(x => x.FromAccount).WithMany(x => x.ExpenseTransactions).HasForeignKey(x => x.FromAccountId);
            builder.HasCheckConstraint("CK_Transactions_AccountsAreNotEqual",
                @$"({nameof(Transaction.FromAccountId)} IS NOT NULL OR {nameof(Transaction.ToAccountId)} IS NOT NULL)" +
                $" AND {nameof(Transaction.FromAccountId)} <> {nameof(Transaction.ToAccountId)}");
        }
    }
}
