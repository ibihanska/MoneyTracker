using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Persistence.Configurations
{

    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Balance).HasColumnType("money").IsRequired();
            builder.Property(x => x.AccountType).HasConversion(v => v.ToString(), v => (AccountType)Enum.Parse(typeof(AccountType), v)).IsRequired();
            builder.Property(x => x.UserEmail).IsRequired();
        }
    }
}
