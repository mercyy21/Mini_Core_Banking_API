using Domain.Domain.Enums;
using Domain.DTO;
using Domain.Entity;
using FluentValidation;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.AccountCommand
{
    public sealed record DeactivateAccountCommand(Guid AccountId) : IRequest<ResponseModel>;
    public sealed class DeactivateAccountCommandHandler : IRequestHandler<DeactivateAccountCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IValidator<DeactivateAccountCommand> _validator;
        public DeactivateAccountCommandHandler(IMiniCoreBankingDbContext context, IValidator<DeactivateAccountCommand> validator )
        {
            _context = context;
            _validator = validator;
        }
        public async Task<ResponseModel> Handle(DeactivateAccountCommand command, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow( command );
            //Deactivate Account
            Account existingAccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == command.AccountId);

            if (existingAccount == null)
            {
                return new ResponseModel { Message = "Account does not exist", Success = false };
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

