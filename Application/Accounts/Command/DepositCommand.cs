using Application.TransactionHistory;
using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.DTO;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.ResultType;

namespace Application.Accounts.AccountCommand
{
    public sealed record DepositCommand(TransactDTO TransactDTO) : IRequest<ResultType.Result>;
    public sealed class DepositCommandHandler : IRequestHandler<DepositCommand, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMediator _mediator;
        private readonly IDecrypt _decrypt;
        public DepositCommandHandler(IMiniCoreBankingDbContext context, IMediator mediator, IDecrypt decrypt)
        {
            _context = context;
            _mediator = mediator;
            _decrypt = decrypt;
        }
        public async Task<ResultType.Result> Handle(DepositCommand command, CancellationToken cancellationToken)
        {
            //Deposit Money
            //Decrypt Transaction Details
            string decryptedTransactionDetails = _decrypt.Decrypt(command.TransactDTO.TransactionDetails);
            //Check if signature contains + (to be used as a delimiter)
            if (command.TransactDTO.Amount <= 0)
            {
                return Result.Failure<DepositCommand>("Amount must be greater than zero");

            }
            if (!decryptedTransactionDetails.Contains("+"))
            {
                return Result.Failure<DepositCommand>("Invalid Signature");
            }
            string[] parts = decryptedTransactionDetails.Split('+');
            string accountNumber = parts[0];
            string email = parts[1];
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
            if (existingAccount == null)
            {
                return ResultType.Result.Failure<DepositCommand>("Account does not exist");
            }
            if(existingAccount.Status == Status.Inactive)
            {
                return ResultType.Result.Failure<DepositCommand>("Account is inactive");
            }
            Customer existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == existingAccount.CustomerId);
            if (existingCustomer.Email!= email)
            {
                return ResultType.Result.Failure<DepositCommand>("Email does not match");
            }

            existingAccount.Balance += command.TransactDTO.Amount;
            _context.Accounts.Update(existingAccount);
            //Record Transaction 
            TransactionHistoryDTO transactionDTO = new TransactionHistoryDTO();
            transactionDTO.Amount = command.TransactDTO.Amount;
            transactionDTO.CustomerId = existingAccount.CustomerId;
            transactionDTO.TransactAt = DateTime.Now;
            transactionDTO.TransactionType = TransactionType.Credit;
            transactionDTO.TransactionTypeDesc= TransactionType.Credit.ToString();
            transactionDTO.Narration = NarrationType.Deposit;
            transactionDTO.NarrationDesc = NarrationType.Deposit.ToString();

            transactionDTO.ReceiversAccountNumber = existingAccount.AccountNumber;
            //Record transactionDTO.
            await _mediator.Send(new RecordTransactionCommand(transactionDTO));
            await _context.SaveChangesAsync(cancellationToken);

            return ResultType.Result.Success<DeleteAccountCommand>("Amount successfully deposited");

        }
    }
}

