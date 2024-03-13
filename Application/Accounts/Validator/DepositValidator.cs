using Application.Accounts.AccountCommand;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class DepositValidator: AbstractValidator<DepositCommand>
    {
        public DepositValidator() 
        {
            RuleFor(c => c.AccountId).NotEmpty().NotNull();
            RuleFor(c => c.Amount).NotEmpty().NotNull().GreaterThan(0).WithMessage("Must be greater than zero");
        }
    }
}
