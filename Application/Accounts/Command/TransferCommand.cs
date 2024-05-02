using Application.TransactionHistory;
using Domain.Domain.Entity;
using Domain.Domain.Enums;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Accounts.Command
{
    public sealed record TransferCommand(TransferDTO TransferDTO) : IRequest<ResponseModel>;

    public sealed class TrasferCommandHandler : IRequestHandler<TransferCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _bankingDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public TrasferCommandHandler(IMiniCoreBankingDbContext bankingDbContext, IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _bankingDbContext = bankingDbContext;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<ResponseModel> Handle(TransferCommand command, CancellationToken cancellationToken)
        {
            Account receiversAccount = await _bankingDbContext.Accounts.FirstOrDefaultAsync(x => x.AccountNumber == command.TransferDTO.AccountNumber);
            if (receiversAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            string customerId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                // User ID not found in authentication context (unlikely scenario)
                return new ResponseModel { Message = "Customer's ID not found", Success = false };
            }
            Account sendersAccount = await _bankingDbContext.Accounts.FirstOrDefaultAsync(x => x.CustomerId == Guid.Parse(customerId));
            if(receiversAccount.AccountNumber == sendersAccount.AccountNumber)
            {
                return new ResponseModel { Message = "Cannot transfer to self", Success=false };
            }
            if(sendersAccount.Status== Status.Inactive.ToString() || receiversAccount.Status == Status.Inactive.ToString())
            {
                return new ResponseModel { Message = "Account is inactive", Success = false };
            }
            if (sendersAccount.Balance < command.TransferDTO.Amount)
            {
                return new ResponseModel { Message = "Insufficient Funds", Success = false };
            }
            sendersAccount.Balance -= command.TransferDTO.Amount;
            receiversAccount.Balance += command.TransferDTO.Amount;
            _bankingDbContext.Accounts.Update(sendersAccount);
            _bankingDbContext.Accounts.Update(receiversAccount);

            //Record Transaction 
            TransactionHistoryDTO transactionDTO = new TransactionHistoryDTO();
            transactionDTO.Amount = command.TransferDTO.Amount;
            transactionDTO.CustomerId = Guid.Parse(customerId);
            transactionDTO.TransactAt = DateTime.Now;
            transactionDTO.TransactionType = TransactionType.Debit;
            transactionDTO.NarrationType = NarrationType.Transfer;
            transactionDTO.SendersAccountNumber = sendersAccount.AccountNumber;
            transactionDTO.ReceiversAccountNumber= receiversAccount.AccountNumber;
            //Record transaction
            await _mediator.Send(new RecordTransactionCommand(transactionDTO));

            await _bankingDbContext.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Transfer Success", Success = true };
        }
    }
}
