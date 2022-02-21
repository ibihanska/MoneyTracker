using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.AccountAggregate
{
    public class Transaction : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid? FromAccountId { get; private set; }
        public Account? FromAccount { get; private set; }
        public Guid? ToAccountId { get; private set; }
        public Account? ToAccount { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Tag? Tag { get; private set; }
        public TransactionType TransactionType => FromAccountId.HasValue && ToAccountId.HasValue ? TransactionType.Transfer :
            FromAccountId.HasValue ? TransactionType.Expense : TransactionType.Income;
        public decimal Amount { get; private set; }
        public string? Note { get; private set; }

        private Transaction() { }

        public Transaction(Guid? fromAccountId, Guid? toAccountId, string? tag, decimal amount, string? note, DateTime transactionDate)
        {
            if (fromAccountId == toAccountId) throw new ArgumentException($"{nameof(fromAccountId)} has not be equal to {nameof(toAccountId)}");
            Id = Guid.NewGuid();
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Tag = new Tag(tag);
            Amount = amount;
            Note = note;
            TransactionDate = transactionDate;
        }

        public void Update(Guid? fromAccountId, Guid? toAccountId, string? TagName, decimal amount, string note, DateTime transactionDate)
        {
            if (fromAccountId == toAccountId) throw new ArgumentException($"{nameof(fromAccountId)} has not be equal to {nameof(toAccountId)}");
            if (this.TransactionType == TransactionType.Expense)
            {
                this.FromAccount.RemoveExpenseTransaction(this);
            }
            else if (this.TransactionType == TransactionType.Income)
            {
                this.ToAccount.RemoveIncomeTransaction(this);
            }
            else
            {
                this.FromAccount.RemoveExpenseTransaction(this);
                this.ToAccount.RemoveIncomeTransaction(this);
            }
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Tag = new Tag(TagName);
            Amount = amount;
            Note = note;
            TransactionDate = transactionDate;
            if (this.TransactionType == TransactionType.Expense)
            {
                this.FromAccount.AddExpenseTransaction(this);
            }
            else if (this.TransactionType == TransactionType.Income)
            {
                this.ToAccount.AddIncomeTransaction(this);
            }
            else
            {
                this.FromAccount.AddExpenseTransaction(this);
                this.ToAccount.AddIncomeTransaction(this);
            }
        }

    }
}
