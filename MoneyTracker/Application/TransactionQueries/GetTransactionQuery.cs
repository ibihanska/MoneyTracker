using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Application.Common.Interfaces;
using MoneyTracker.Domain.AccountAggregate;

namespace MoneyTracker.Application.TransactionQueries
{
    public class GetTransactionQuery : IRequest<TransactionDto>
    {
        public Guid Id { get; set; }

        public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, TransactionDto>
        {
            private readonly IMoneyTrackerDbContext _context;
            private readonly IMapper _mapper;

            public GetTransactionQueryHandler(IMoneyTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper=mapper;
            }

            public async Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
            {
                var transaction = _context.Transactions.AsNoTracking()
                        .FirstOrDefault(c=>c.Id==request.Id);
                if (transaction == null)
                {
                    throw new NotFoundException(nameof(Transaction), request.Id);
                }
                var transactionDto = _mapper.Map<TransactionDto>(transaction);
                return transactionDto;
            }
        }
    }
}
