using Application.Accounts.AccountCommand;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class CreateAccountValidator: AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator() 
        { 
            RuleFor(c=> c.AccountDTO.CustomerId).NotNull().NotEmpty();
            RuleFor(c=>c.AccountDTO.AccountType).NotEmpty().NotNull();
        }
    }
}
