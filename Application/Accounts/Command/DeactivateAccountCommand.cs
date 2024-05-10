using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.Interfaces;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DeactivateAccountCommand(Guid AccountId) : IRequest<Result>;
    public sealed class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public DeactivateAccountCommandHandler(IMiniCoreBankingDbContext context )
        {
            _context = context;
        }
        public async Task<Result> Handle(DeactivateAccountCommand command, CancellationToken cancellationToken)
        {
            //Deactivate Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);

            if (existingAccount == null)
            {
                return Result.Failure<DeactivateAccountCommand>("Account does not exist");
            }
            if(existingAccount.Status==Status.Inactive)
            {
                return Result.Failure<DeactivateAccountCommand>("Account is already inactive");
            }

            existingAccount.ClosedAt = DateTime.Now;
            existingAccount.UpdatedAt = DateTime.Now;
            existingAccount.Status = Status.Inactive;
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success<DeactivateAccountCommand>("Account Deactivated");
        }
    }
}

