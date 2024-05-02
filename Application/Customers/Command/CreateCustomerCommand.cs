using MediatR;
using Domain.DTO;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Accounts.AccountCommand;
using Application.Customers.Helper;
using Domain.Domain.Enums;
using Application.Customers.PasswordHasher;
using Domain.Domain.Entity;

namespace Application.Customers.CustomerCommand
{
    public sealed record CreateCustomerCommand(CreateCustomerDTO CustomerDTO) : IRequest<MultipleDataResponseModel>;

    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, MultipleDataResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateCustomerCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<MultipleDataResponseModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            CustomerHelper helper = new CustomerHelper(); 

            //Create Customer
            var existingCustomer = await _context.Customers.AnyAsync(x => x.Email == request.CustomerDTO.Email);
            if (existingCustomer)
            {
                return new MultipleDataResponseModel { Message = "Customer already exists", Success = false };
            }
            //Hashing Password
            Hasher hasher = new Hasher();
            var (hashedPassword, salt)=hasher.HashPassword(request.CustomerDTO.Password);

            Customer entity = new Customer
            {
                FirstName = request.CustomerDTO.FirstName,
                LastName = request.CustomerDTO.LastName,
                Email = request.CustomerDTO.Email,
                Address = request.CustomerDTO.Address,
                PhoneNumber = request.CustomerDTO.PhoneNumber,
                Password = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.Now,
                Status = Status.Active.ToString()
            };

            //Save to Database
            await _context.Customers.AddAsync(entity, CancellationToken.None);
            await _context.SaveChangesAsync(CancellationToken.None);

            CustomerResponseDTO customerDTO = _mapper.Map<CustomerResponseDTO>(helper.GetCustomerResponse(request.CustomerDTO, entity.Id));

            //Create Account 
            AccountDTO newAccount = new AccountDTO
            {
                CustomerId = entity.Id,
                AccountType = request.CustomerDTO.AccountType
            };
            ResponseModel response = await _mediator.Send(new CreateAccountCommand(newAccount), cancellationToken);

            return new MultipleDataResponseModel { Data = new List<object> { customerDTO, response.Data }, 
                Message = "Customer created successfully", Success = true };
        }
    }
}
