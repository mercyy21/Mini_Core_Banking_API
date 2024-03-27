using Domain.Domain.Enums;
using Domain.DTO;
using Domain.Entity;
using FluentValidation;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{

    public sealed record ActivateAccountCommand(Guid AccountId) : IRequest<ResponseModel>;
    public sealed class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public ActivateAccountCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel> Handle(ActivateAccountCommand command, CancellationToken cancellationToken)
        {
            //Activate Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);
            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }

            existingAccount.UpdatedAt = DateTime.Now;
            existingAccount.Status = Status.Active.ToString();
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Account activated", Success = true };
        }
    }
}

