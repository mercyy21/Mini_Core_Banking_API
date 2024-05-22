using Application.Domain.Entity;
using Application.Interfaces;
using Application.ResultType;
using MediatR;

namespace Application.TransactionHistory
{
    public sealed record DeleteTransactionHistoryCommand(): IRequest<Result>;

    public sealed class DeleteTransactionHistoryCommandHandler: IRequestHandler<DeleteTransactionHistoryCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;

        public DeleteTransactionHistoryCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteTransactionHistoryCommand command, CancellationToken cancellationToken)
        {
            var transactionlist= _context.Transactions.ToList();

            foreach(var transaction in transactionlist)
            {
                _context.Transactions.Remove(transaction);
            }
            _context.SaveChangesAsync(cancellationToken);
            return Result.Success<DeleteTransactionHistoryCommand>("Deleted Successfully");
        }
    }
}
