using Application.Domain.Entity;
using Application.DTO;
using Application.Interfaces;
using Application.ResultType;
using MediatR;

namespace Application.TransactionHistory;

public sealed record RecordTransactionCommand(TransactionHistoryDTO HistoryDTO) : IRequest<ResultType.Result>;

public sealed class RecordTransactionCommandHandler : IRequestHandler<RecordTransactionCommand, ResultType.Result>
{
    private readonly IMiniCoreBankingDbContext _context;

    public async Task<ResultType.Result> Handle(RecordTransactionCommand command, CancellationToken cancellationToken)
    {
        Transaction transaction = new Transaction
        {
            TransactionType = command.HistoryDTO.TransactionType,
            Narration = command.HistoryDTO.NarrationType,
            Amount = command.HistoryDTO.Amount,
            Timestamp = DateTime.Now,
            CustomerId = command.HistoryDTO.CustomerId,
            SendersAccountNumber = command.HistoryDTO.SendersAccountNumber,
            ReceiversAccountNumber = command.HistoryDTO.ReceiversAccountNumber
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return ResultType.Result.Success<RecordTransactionCommand>("Transaction recorded successfully", transaction);
    }
}