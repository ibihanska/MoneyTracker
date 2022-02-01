using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.AccountAggregate
{
    public class Account:AuditableEntity
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public decimal Balance { get; private set; }
        public AccountType AccountType { get; private set; }
        public string UserEmail { get; private set; }

        public List<Transaction> Transactions { get; private set; }
        public Account(string name, decimal balance, AccountType accountType, string userEmail)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = balance;
            AccountType = accountType;
            UserEmail = userEmail;
            Transactions = new List<Transaction>();
        }

        public void AddTransaction(Guid fromAccountId, Guid toAccountId, Tag tag, TransactionType transactionType, decimal amount, string note, DateTime transactionDate)
        {
            var transaction = new Transaction(fromAccountId, toAccountId, tag, transactionType, amount, note, transactionDate);
            Transactions.Add(transaction);
        }
        public Transaction GetTransaction(Guid transactionId)
        {
            var transaction = Transactions.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Transaction with this Id doesn't exist!");
            }
            return transaction;
        }
        public void UpdateTransaction(Guid transactionId, Guid fromAccountId, Guid toAccountId, Tag tag, TransactionType transactionType, decimal amount, string note, DateTime transactionDate)
        {
            var transaction = Transactions.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Transaction with this Id doesn't exist!");
            }
            transaction.UpdateTransactionData(fromAccountId, toAccountId, tag, transactionType, amount, note, transactionDate);
        }
        public void DeleteTransaction(Guid transactionId)
        {
            var transaction = Transactions.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            if (transaction == null)
            {
                throw new Exception("Transaction with this Id doesn't exist!");
            }
            Transactions.Remove(transaction);
        }
    }
}
