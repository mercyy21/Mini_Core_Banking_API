using Application.Domain.Entity;
using Application.DTO;
using Application.Interfaces;
using Application.ResultType;
using MediatR;

namespace Application.TransactionHistory;

public sealed record RecordTransactionCommand(TransactionHistoryDTO HistoryDTO) : IRequest<Result>;

public sealed class RecordTransactionCommandHandler : IRequestHandler<RecordTransactionCommand, Result>
{
    private readonly IMiniCoreBankingDbContext _context;

    public async Task<Result> Handle(RecordTransactionCommand command, CancellationToken cancellationToken)
    {
        Transaction transaction = new Transaction
        {
            TransactionType = command.HistoryDTO.TransactionType,
            TransactionTypeDesc = command.HistoryDTO.TransactionTypeDesc,
            Narration = command.HistoryDTO.Narration,
            NarrationDesc= command.HistoryDTO.NarrationDesc,
            Amount = command.HistoryDTO.Amount,
            Timestamp = DateTime.Now,
            CustomerId = command.HistoryDTO.CustomerId,
            SendersAccountNumber = command.HistoryDTO.SendersAccountNumber,
            ReceiversAccountNumber = command.HistoryDTO.ReceiversAccountNumber
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success<RecordTransactionCommand>("Transaction recorded successfully", transaction);
    }
}