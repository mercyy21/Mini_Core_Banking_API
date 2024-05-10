using Application.Accounts.AccountCommand;
using Application.Domain.Entity;
using Application.Interfaces;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.CustomerCommand
{
    public sealed record DeleteCustomerCommand(Guid CustomerId) : IRequest<Result>;

    public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMediator _mediator;
        public DeleteCustomerCommandHandler(IMiniCoreBankingDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            //Delete Customer
            Customer existingUser = await _context.Customers.FirstOrDefaultAsync(x => x.Id == command.CustomerId);
            if (existingUser == null)
            {
                return Result.Success<DeleteCustomerCommandHandler>("Customer does not exist");
            }
            await _mediator.Send(new DeleteAccountCommand(command.CustomerId));
            _context.Customers.Remove(existingUser);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Failure<DeleteCustomerCommandHandler>("Deleted Successfully");


        }
    }
}

