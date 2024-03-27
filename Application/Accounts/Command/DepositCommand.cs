using Domain.DTO;
using Domain.Entity;
using FluentValidation;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DepositCommand(Guid AccountId, double Amount) : IRequest<ResponseModel>;
    public sealed class DepositCommandHandler : IRequestHandler<DepositCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public DepositCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel> Handle(DepositCommand command, CancellationToken cancellationToken)
        {
            //Deposit Money
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);
            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            if (command.Amount <= 0)
            {
                return new ResponseModel { Message = "Amount must be greater than zero", Success = false };

            }
            existingAccount.Balance += command.Amount;
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Amount successfully deposited", Success = true };

        }
    }
}

