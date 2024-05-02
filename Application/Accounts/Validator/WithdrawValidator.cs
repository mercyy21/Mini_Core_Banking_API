using Application.Accounts.AccountCommand;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class WithdrawValidator: AbstractValidator<WithdrawCommand>
    {
        public WithdrawValidator() 
        {
            RuleFor(c=>c.TransactDTO.Signature).NotEmpty().NotNull();
            RuleFor(c=> c.TransactDTO.Amount).NotEmpty().NotNull().GreaterThan(0).WithMessage("Must be greater than zero");
        }
    }
}
