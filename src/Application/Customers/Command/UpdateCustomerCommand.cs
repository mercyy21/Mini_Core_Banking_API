using AutoMapper;
using Application.Domain.Entity;
using Application.ResultType;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.DTO;

namespace Application.Customers.CustomerCommand
{
    public sealed record UpdateCustomerCommand(Guid CustomerId, CustomerDTO Customer) : IRequest<Result>;

    public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public UpdateCustomerCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            //Update Customer
            Customer existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == command.CustomerId);
            if (existingCustomer == null)
            {
                return Result.Failure<UpdateCustomerCommand>("Customer does not exist");
            }
            existingCustomer.FirstName = command.Customer.FirstName;
            existingCustomer.LastName = command.Customer.LastName;
            existingCustomer.Email = command.Customer.Email;
            existingCustomer.Address = command.Customer.Address;
            existingCustomer.PhoneNumber = command.Customer.PhoneNumber;
            existingCustomer.UpdatedAt = DateTime.Now;
            _context.Customers.Update(existingCustomer);
            await _context.SaveChangesAsync(cancellationToken);

            //Response
            CustomerResponseDTO responseDTO = _mapper.Map<CustomerResponseDTO>(existingCustomer);
            return Result.Success<UpdateCustomerCommand>("Customer Updated Successfully",responseDTO);
        }

    }
}


