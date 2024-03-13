using Application.Accounts.AccountCommand;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class ActivateAccountValidator: AbstractValidator<ActivateAccountCommand>
    {
        public ActivateAccountValidator()
        {
            RuleFor(c => c.AccountId).NotEmpty().NotNull();
        }
    }
}
