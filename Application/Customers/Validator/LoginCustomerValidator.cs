using Application.Customers.Command;
using FluentValidation;

namespace Application.Customers.Validator
{
    public class LoginCustomerValidator : AbstractValidator<LoginCustomerCommand>
    {
        public LoginCustomerValidator()
        {
            RuleFor(c => c.Email).NotEmpty().NotNull();
            RuleFor(c => c.Password).NotEmpty().NotNull();
        }
    }
}
