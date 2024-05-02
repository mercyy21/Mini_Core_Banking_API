using Application.Accounts.AccountCommand;
using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.CustomerCommand
{
    public sealed record DeleteCustomerCommand(Guid CustomerId) : IRequest<ResponseModel>;

    public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMediator _mediator;
        public DeleteCustomerCommandHandler(IMiniCoreBankingDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<ResponseModel> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            //Delete Customer
            Customer existingUser = await _context.Customers.FirstOrDefaultAsync(x => x.Id == command.CustomerId);
            if (existingUser == null)
            {
                return new ResponseModel { Message = "Customer does not exist", Success = false };
            }
            await _mediator.Send(new DeleteAccountCommand(command.CustomerId));
            _context.Customers.Remove(existingUser);
            await _context.SaveChangesAsync(cancellationToken);
            return new ResponseModel { Message = "Deleted Successfully", Success = true };


        }
    }
}

