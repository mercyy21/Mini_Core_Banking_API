using Application.Customers.CustomerCommand;
using FluentValidation;

namespace Application.Customers.Validator
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(c=> c.CustomerDTO.FirstName).NotEmpty().NotNull();
            RuleFor(c => c.CustomerDTO.LastName).NotEmpty().NotNull();
            RuleFor(c => c.CustomerDTO.Email).NotEmpty().NotNull();
            RuleFor(c => c.CustomerDTO.Address).NotEmpty().NotNull();   
            RuleFor(c => c.CustomerDTO.PhoneNumber).NotEmpty().NotNull();

        }

    }
}
