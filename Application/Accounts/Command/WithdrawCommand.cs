using Application.TransactionHistory;
using Domain.Domain.Entity;
using Domain.Domain.Enums;
using Domain.DTO;
using Domain.Interfaces;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record WithdrawCommand(TransactDTO TransactDTO) : IRequest<ResponseModel>;
    public sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, ResponseModel>
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
        public async Task<ResponseModel> Handle(WithdrawCommand command, CancellationToken cancellationToken)
        {
            //Withdraw Money
            //Decrypt Signature
            string decryptedSignature = _decrypt.Decrypt(command.TransactDTO.Signature);
            //Check if signature contains + (to be used as a delimiter)
            if (!decryptedSignature.Contains("+"))
            {
                return new ResponseModel { Message = "Invalid Signature", Success = false };
            }
            string[] parts = decryptedSignature.Split('+');
            string accountNumber = parts[0];
            string email = parts[1];
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            if (existingAccount.Status == Status.Inactive.ToString())
            {
                return new ResponseModel { Message = "Account is inactive", Success = false };
            }
            Customer existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == existingAccount.CustomerId);
            if (existingCustomer.Email != email)
            {
                return new ResponseModel { Message = "Email does not match", Success = false };
            }
            if (command.TransactDTO.Amount > existingAccount.Balance)
            {
                return new ResponseModel { Message = "Amount exceeds balance", Success = false };
            }

            existingAccount.Balance -= command.TransactDTO.Amount;
            _context.Accounts.Update(existingAccount);

            //Record Transaction 
            TransactionHistoryDTO transactionDTO = new TransactionHistoryDTO();
            transactionDTO.Amount = command.TransactDTO.Amount;
            transactionDTO.CustomerId = existingAccount.CustomerId;
            transactionDTO.TransactAt = DateTime.Now;
            transactionDTO.TransactionType =TransactionType.Debit;
            transactionDTO.NarrationType = NarrationType.Withdraw;
            transactionDTO.SendersAccountNumber = existingAccount.AccountNumber;
            await _mediator.Send(new RecordTransactionCommand(transactionDTO));

            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Amount successfully withdrawn", Success = true };
        }
    }
}

