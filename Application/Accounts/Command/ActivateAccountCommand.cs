using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.Interfaces;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{

    public sealed record ActivateAccountCommand(Guid AccountId) : IRequest<Result>;
    public sealed class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public ActivateAccountCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ActivateAccountCommand command, CancellationToken cancellationToken)
        {
            //Activate Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);
            if (existingAccount == null)
            {
                return Result.Failure<ActivateAccountCommand>("Account does not exist");
            }
            if (existingAccount.Status == Status.Active)
            {
                return Result.Failure<ActivateAccountCommand>("Account is already active");
            }

            existingAccount.UpdatedAt = DateTime.Now;
            existingAccount.Status = Status.Active;
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success<ActivateAccountCommand>("Account activated");
        }
    }
}

