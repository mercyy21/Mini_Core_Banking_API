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
    public sealed record WithdrawCommand(TransactDTO TransactDTO) : IRequest<ResultType.Result>;
    public sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IDecrypt _decrypt;
        private readonly IMediator _mediator;
        public WithdrawCommandHandler(IMiniCoreBankingDbContext context, IDecrypt decrypt, IMediator mediator)
        {
            _context = context;
            _decrypt = decrypt;
            _mediator = mediator;
        }
        public async Task<ResultType.Result> Handle(WithdrawCommand command, CancellationToken cancellationToken)
        {
            //Withdraw Money
            //Decrypt Signature
            string decryptedSignature = _decrypt.Decrypt(command.TransactDTO.TransactionDetails);
            
            //Check if signature contains + (to be used as a delimiter)
            if (!decryptedSignature.Contains("+"))
            {
                return Result.Failure<WithdrawCommand>("Invalid Signature");
            }
            string[] parts = decryptedSignature.Split('+');
            string accountNumber = parts[0];
            string email = parts[1];
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
            if (existingAccount == null)
            {
                return Result.Success<WithdrawCommand>("Account does not exist");
            }
            if (command.TransactDTO.Amount > existingAccount.Balance)
            {
                return Result.Failure<WithdrawCommand>("Amount exceeds balance");
            }

            
            if (existingAccount.Status == Status.Inactive)
            {
                return Result.Failure<WithdrawCommand>("Account is inactive");
            }
            Customer existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == existingAccount.CustomerId);
            if (existingCustomer.Email != email)
            {
                return Result.Failure<WithdrawCommand>("Email does not match");
            }
      

            existingAccount.Balance -= command.TransactDTO.Amount;
            _context.Accounts.Update(existingAccount);

            //Record Transaction 
            TransactionHistoryDTO transactionDTO = new TransactionHistoryDTO();
            transactionDTO.Amount = command.TransactDTO.Amount;
            transactionDTO.CustomerId = existingAccount.CustomerId;
            transactionDTO.TransactAt = DateTime.Now;
            transactionDTO.TransactionType =TransactionType.Debit;
            transactionDTO.TransactionTypeDesc = TransactionType.Debit.ToString();
            transactionDTO.Narration = NarrationType.Withdraw;
            transactionDTO.NarrationDesc = NarrationType.Withdraw.ToString();
            transactionDTO.SendersAccountNumber = existingAccount.AccountNumber;
            await _mediator.Send(new RecordTransactionCommand(transactionDTO));

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success<WithdrawCommand>("Amount successfully withdrawn");
        }
    }
}

