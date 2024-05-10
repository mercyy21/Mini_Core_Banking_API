using Application.TransactionHistory;
using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.ResultType;

namespace Application.Accounts.Command
{
    public sealed record TransferCommand(TransferDTO TransferDTO) : IRequest<ResultType.Result>;

    public sealed class TransferCommandHandler : IRequestHandler<TransferCommand, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _bankingDbContext;
        private readonly IMediator _mediator;

        public TransferCommandHandler(IMiniCoreBankingDbContext bankingDbContext, IMediator mediator)
        {
            _bankingDbContext = bankingDbContext;
            _mediator = mediator;
        }

        public async Task<ResultType.Result> Handle(TransferCommand command, CancellationToken cancellationToken)
        {
            Account receiversAccount = await _bankingDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == command.TransferDTO.ReceiversAccountNumber);
            Account sendersAccount = await _bankingDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == command.TransferDTO.SendersAccountNumber);
            if(sendersAccount==null)
            {
                return Result.Failure<TransferCommand>("Senders account does not exist");

            }
            if (receiversAccount == null)
            {
                return Result.Failure<TransferCommand>("Receivers account does not exist");
            }
            if (sendersAccount.Balance < command.TransferDTO.Amount)
            {
                return Result.Failure<TransferCommand>("Insufficient Funds");
            }
            if(receiversAccount.AccountNumber == sendersAccount.AccountNumber)
            {
                return Result.Failure<TransferCommand>("Cannot transfer to self");
            }
            if(sendersAccount.Status== Status.Inactive || receiversAccount.Status == Status.Inactive)
            {
                return Result.Failure<TransferCommand>("Account is inactive");
            }
         
            sendersAccount.Balance -= command.TransferDTO.Amount;
            receiversAccount.Balance += command.TransferDTO.Amount;
            _bankingDbContext.Accounts.Update(sendersAccount);
            _bankingDbContext.Accounts.Update(receiversAccount);

            //Record Transaction 
            TransactionHistoryDTO transactionDTO = new TransactionHistoryDTO();
            transactionDTO.Amount = command.TransferDTO.Amount;
            transactionDTO.CustomerId = sendersAccount.CustomerId;
            transactionDTO.TransactAt = DateTime.Now;
            transactionDTO.TransactionType = TransactionType.Debit;
            transactionDTO.NarrationType = NarrationType.Transfer;
            transactionDTO.SendersAccountNumber = sendersAccount.AccountNumber;
            transactionDTO.ReceiversAccountNumber= receiversAccount.AccountNumber;
            //Record transaction
            await _mediator.Send(new RecordTransactionCommand(transactionDTO));

            await _bankingDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success<TransferCommand>("Transfer Success");
        }
    }
}
