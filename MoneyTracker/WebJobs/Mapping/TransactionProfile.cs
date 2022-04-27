using AutoMapper;
using MoneyTracker.Domain.AccountAggregate;

namespace WebJobs
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>();
        }
    }
}
