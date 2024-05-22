using Application.Domain.Entity;
using Application.Interfaces;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Query
{
    public sealed record ViewCustomerTransaction_HistoryByIdQuery(Guid CustomerId):IRequest<Result>;

    public sealed class ViewCustomerTransaction_HistoryByIdQueryHandler : IRequestHandler<ViewCustomerTransaction_HistoryByIdQuery, Result>
    {
        private readonly IMiniCoreBankingDbContext _dbContext;

        public ViewCustomerTransaction_HistoryByIdQueryHandler(IMiniCoreBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ViewCustomerTransaction_HistoryByIdQuery query, CancellationToken cancellationToken)
        {
            bool customerExists = await _dbContext.Customers.AnyAsync(x=> x.Id == query.CustomerId);
            if(!customerExists)
            {
                return Result.Failure<ViewCustomerTransaction_HistoryByIdQuery>("Customer does not exist");
            }
            //Get Customers Transaction History
            Transaction transactionHistory = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.CustomerId == query.CustomerId);

            //Check if it exists 
            if(transactionHistory == null)
            {
                return Result.Failure<ViewCustomerTransaction_HistoryByIdQuery>("Customer has made zero transactions");
            }
            var customerTransactions = _dbContext.Transactions
                .Where(o => o.CustomerId == query.CustomerId)
                .ToList();

            return Result.Success<ViewCustomerTransaction_HistoryByIdQuery>("Customers transaction history returned successfully", customerTransactions);
        }
    }
}
