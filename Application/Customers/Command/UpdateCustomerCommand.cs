using AutoMapper;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.CustomerCommand
{
    public sealed record UpdateCustomerCommand(Guid CustomerId, CustomerDTO Customer) : IRequest<ResponseModel>;

    public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public UpdateCustomerCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            //Update Customer
            Customer existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == command.CustomerId);
            if (existingCustomer == null)
            {
                return new ResponseModel { Message = "Customer does not exist", Success = false };
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
            return new ResponseModel { Data= responseDTO,Message = "Customer Updated Successfully", Success = true };
        }

    }
}


