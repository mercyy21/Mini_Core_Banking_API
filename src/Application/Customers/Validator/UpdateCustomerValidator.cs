using Application.Customers.CustomerCommand;
using FluentValidation;

namespace Application.Customers.Validator
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty().NotNull();
            RuleFor(x => x.Customer.FirstName).NotEmpty()
                .NotNull();
            RuleFor(x => x.Customer.LastName).NotEmpty();
            RuleFor(x => x.Customer.Email).NotEmpty();
            RuleFor(x => x.Customer.Address).NotEmpty();
            RuleFor(x => x.Customer.PhoneNumber).NotEmpty();




        }
    }
}
