using System;
using MoneyTracker.Domain.AccountAggregate;
using MoneyTracker.Domain.Common;
using Xunit;

namespace MoneyTracker.UnitTests
{
    public class AccountAggregateTest
    {
        private readonly Account _account;
        private readonly DateTime _date;

        public AccountAggregateTest()
        {
            _account = new Account("Cash", 200, 0, "alice@gmail.com");
            _date = new DateTime(2022, 12, 25, 10, 30, 50);
        }

        [Fact]
        public void AddExpenseTransactionWhenBalanceNotEnoughThenConflictExceptionThrown()
        {
            var transaction = new Transaction(Guid.NewGuid(), null, "Products", 201, null, _date);
            Assert.Throws<ConflictException>(() => _account.AddExpenseTransaction(transaction));
        }

        [Fact]
        public void AddExpenseTransactionWhenAccountStateCorrectThenAddedSuccessfully()
        {
            var transaction = new Transaction(Guid.NewGuid(), null, "Products", 50, null, _date);
            _account.AddExpenseTransaction(transaction);
            Assert.Equal(150, _account.Balance);
        }

        [Fact]
        public void RemoveExpenseTransactionWhenAccountStateCorrectThenRemovedSuccessfully()
        {
            var transaction = new Transaction(Guid.NewGuid(), null, "Products", 50, null, _date);
            _account.AddExpenseTransaction(transaction);
            _account.RemoveExpenseTransaction(transaction);
            Assert.Equal(200, _account.Balance);
        }

        [Fact]
        public void RemoveIncomeTransactionWhenBalanceNotEnoughThenConflictExceptionThrown()
        {
            var expenseTransaction = new Transaction(Guid.NewGuid(), null, "Products", 300, null, _date);
            var incomeTransaction = new Transaction(null, Guid.NewGuid(), "Salary", 200, null, _date);
            _account.AddIncomeTransaction(incomeTransaction);
            _account.AddExpenseTransaction(expenseTransaction);
            Assert.Throws<ConflictException>(() => _account.RemoveIncomeTransaction(incomeTransaction));
        }


        [Fact]
        public void CreateTransactionWhenAccountsIdAreEqualThenArgumentExceptionThrown()
        {
            var accountsId = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() => new Transaction(accountsId, accountsId, "Products", 300, null, _date));
        }

        [Fact]
        public void CreateTransactionWhenAmountIsLessThan0ThenArgumentExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => new Transaction(Guid.NewGuid(), Guid.NewGuid(), "Products", -30, null, _date));
        }

        [Fact]
        public void AddIncomeTransactionWhenAccountStateCorrectThenAddedSuccessfully()
        {
            var transaction = new Transaction(null, Guid.NewGuid(), "Schoolarship", 2000, null, _date);
            _account.AddIncomeTransaction(transaction);
            Assert.Equal(2200, _account.Balance);
        }

        [Fact]
        public void AddExpenseTransactionWhenTransactionIsIncomeThenInvalidOperationExceptionThrown()
        {
            var transaction = new Transaction(null, Guid.NewGuid(), "Salary", 2000, null, _date);
            Assert.Throws<InvalidOperationException>(() => _account.AddExpenseTransaction(transaction));
        }

        [Fact]
        public void AddIncomeTransactionWhenTransactionIsExpenseThenInvalidOperationExceptionThrown()
        {
            var transaction = new Transaction(Guid.NewGuid(), null, "Products", 2000, null, _date);
            Assert.Throws<InvalidOperationException>(() => _account.AddIncomeTransaction(transaction));
        }


    }
}
