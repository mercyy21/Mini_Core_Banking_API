using Application.Accounts.Command;
using FluentValidation;

namespace Application.Accounts.Validator
{
    public class TransferValidator : AbstractValidator<TransferCommand>
    {
        public TransferValidator()
        {
            RuleFor(c => c.TransferDTO.SendersAccountNumber).NotEmpty().NotNull();
            RuleFor(c => c.TransferDTO.ReceiversAccountNumber).NotEmpty().NotNull();
            RuleFor(c => c.TransferDTO.Amount).NotEmpty().NotNull().GreaterThan(0).WithMessage("Must be greater than zero");
        }
    }
}
