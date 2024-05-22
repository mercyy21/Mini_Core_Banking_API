using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.Interfaces;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DeleteAccountCommand(Guid CustomerId) : IRequest<Result>;
    public sealed class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public DeleteAccountCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            //Delete Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.CustomerId == command.CustomerId);
            if (existingAccount == null)
            {
                return Result.Failure<DeleteAccountCommand>("Account does not exist");
            }
            existingAccount.Status= Status.Removed;
            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Failure<DeleteAccountCommand>("Account Deleted Successfully");
        }
    }
}

