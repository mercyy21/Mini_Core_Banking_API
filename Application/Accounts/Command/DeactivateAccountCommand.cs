using Domain.Domain.Entity;
using Domain.Domain.Enums;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DeactivateAccountCommand(Guid AccountId) : IRequest<ResponseModel>;
    public sealed class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public DeactivateAccountCommandHandler(IMiniCoreBankingDbContext context )
        {
            _context = context;
        }
        public async Task<ResponseModel> Handle(DeactivateAccountCommand command, CancellationToken cancellationToken)
        {
            //Deactivate Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);

            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            if(existingAccount.Status==Status.Inactive.ToString())
            {
                return new ResponseModel { Message = "Account is already inactive", Success = false };
            }

            existingAccount.ClosedAt = DateTime.Now;
            existingAccount.UpdatedAt = DateTime.Now;
            existingAccount.Status = Status.Inactive.ToString();
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Account Deactivated", Success = true };
        }
    }
}

