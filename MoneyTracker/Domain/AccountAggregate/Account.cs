using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.AccountAggregate
{
    public class Account : AuditableEntity
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public decimal Balance { get; private set; }
        public AccountType AccountType { get; private set; }
        public string UserEmail { get; private set; }
        private List<Transaction> _incomeTransactions;
        private List<Transaction> _exspenseTransactions;
        public IReadOnlyCollection<Transaction> ExpenseTransactions
        {
            get => _exspenseTransactions;
            private set => _exspenseTransactions = value.ToList();
        }
        public IReadOnlyCollection<Transaction> IncomeTransactions
        {
            get => _incomeTransactions;
            private set => _incomeTransactions = value.ToList();
        }
        public Account(string name, decimal balance, AccountType accountType, string userEmail)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = balance;
            AccountType = accountType;
            UserEmail = userEmail;
            IncomeTransactions = new List<Transaction>();
            ExpenseTransactions = new List<Transaction>();
        }

        public void Update(string name, AccountType accountType)
        {
            Name = name;
            AccountType = accountType;
        }

        public void AddExpenseTransaction(Transaction transaction)
        {
            _exspenseTransactions.Add(transaction);
            if(this.Balance-transaction.Amount<0)
            {
                throw new ConflictException("There are not enough money on this account");
            }
            this.Balance -= transaction.Amount;
        }

        public void AddIncomeTransaction(Transaction transaction)
        {
            _incomeTransactions.Add(transaction);
            this.Balance += transaction.Amount;
        }

        public void RemoveExpenseTransaction(Transaction transaction)
        {
            _exspenseTransactions.Remove(transaction);
            this.Balance += transaction.Amount;
        }

        public void RemoveIncomeTransaction(Transaction transaction)
        {
            _incomeTransactions.Remove(transaction);
            this.Balance -= transaction.Amount;
        }
    }
}
