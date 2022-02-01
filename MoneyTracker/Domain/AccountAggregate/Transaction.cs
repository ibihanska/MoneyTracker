using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.AccountAggregate
{
    public class Transaction:AuditableEntity
    {
        public Guid TransactionId { get; private set; }
        public Guid FromAccountId { get; private set; }
        public Guid? ToAccountId { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public Tag Tag { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public decimal Amount { get; private set; }
        public string Note { get; private set; }
        public Transaction(Guid fromAccountId, Guid toAccountId, Tag tag, TransactionType transactionType, decimal amount, string note, DateTime transactionDate)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            TransactionId = Guid.NewGuid();
            Tag = tag;
            TransactionType = transactionType;
            Amount = amount;
            Note = note;
            TransactionDate = transactionDate;
        }
        public void UpdateTransactionData(Guid fromAccountId, Guid toAccountId, Tag tag, TransactionType transactionType, decimal amount, string note, DateTime transactionDate)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Tag = tag;
            TransactionType = transactionType;
            Amount = amount;
            Note = note;
            TransactionDate = transactionDate;
        }

    }
}
