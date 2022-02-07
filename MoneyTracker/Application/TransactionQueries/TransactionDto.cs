using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.Application.Common.Mapping;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.TransactionQueries
{
    public class TransactionDto: IMapFrom<Transaction>
    {
        public Guid Id { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Tag? Tag { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }

    }
}
