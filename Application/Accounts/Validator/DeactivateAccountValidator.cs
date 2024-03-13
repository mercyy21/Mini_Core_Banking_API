using Application.Accounts.AccountCommand;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class DeactivateAccountValidator: AbstractValidator<DeactivateAccountCommand>
    {
        public DeactivateAccountValidator() 
        {
            RuleFor(c=> c.AccountId).NotEmpty().NotNull();
        }
    }
}
