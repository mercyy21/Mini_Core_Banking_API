using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;

namespace Application.TransactionHistory;

public sealed record RecordTransactionCommand(TransactionHistoryDTO HistoryDTO) : IRequest<ResponseModel>;

public sealed class RecordTransactionCommandHandler : IRequestHandler<RecordTransactionCommand, ResponseModel>
{
    private readonly IMiniCoreBankingDbContext _context;

    public async Task<ResponseModel> Handle(RecordTransactionCommand command, CancellationToken cancellationToken)
    {
        Transaction transaction = new Transaction
        {
            TransactionType = command.HistoryDTO.TransactionType.ToString(),
            Narration = command.HistoryDTO.NarrationType.ToString(),
            Amount = command.HistoryDTO.Amount,
            Timestamp = DateTime.Now,
            CustomerId = command.HistoryDTO.CustomerId,
            SendersAccountNumber = command.HistoryDTO.SendersAccountNumber,
            ReceiversAccountNumber = command.HistoryDTO.ReceiversAccountNumber
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return new ResponseModel { Data = transaction, Message = "Transaction recorded successfully", Success= true };
    }
}