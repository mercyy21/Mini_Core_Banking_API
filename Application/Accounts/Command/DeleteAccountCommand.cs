using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DeleteAccountCommand(Guid CustomerId) : IRequest<ResponseModel>;
    public sealed class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        public DeleteAccountCommandHandler(IMiniCoreBankingDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseModel> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            //Delete Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.CustomerId == command.CustomerId);
            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
            }
            _context.Accounts.Remove(existingAccount);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Account Deleted Successfully", Success = true };
        }
    }
}

