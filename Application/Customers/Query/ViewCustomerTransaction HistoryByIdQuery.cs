using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Query
{
    public sealed record ViewCustomerTransaction_HistoryByIdQuery(Guid CustomerId):IRequest<ResponseModel>;

    public sealed class ViewCustomerTransaction_HistoryByIdQueryHandler : IRequestHandler<ViewCustomerTransaction_HistoryByIdQuery, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _dbContext;

        public ViewCustomerTransaction_HistoryByIdQueryHandler(IMiniCoreBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseModel> Handle(ViewCustomerTransaction_HistoryByIdQuery query, CancellationToken cancellationToken)
        {
            bool customerExists = await _dbContext.Customers.AnyAsync(x=> x.Id == query.CustomerId);
            if(!customerExists)
            {
                return new ResponseModel { Message="Customer does not exist", Success=false };
            }
            //Get Customers Transaction History
            Transaction transactionHistory = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.CustomerId == query.CustomerId);

            //Check if it exists 
            if(transactionHistory == null)
            {
                return new ResponseModel { Message = "Customer has made zero transactions", Success = false };
            }
            var customerTransactions = _dbContext.Transactions
                .Where(o => o.CustomerId == query.CustomerId)
                .ToList();

            return new ResponseModel { Data = customerTransactions, Message = "Customers transaction history returned successfully", Success = true };
        }
    }
}
