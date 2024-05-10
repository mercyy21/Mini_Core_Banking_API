using MediatR;
using Application.DTO;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Accounts.AccountCommand;
using Application.Customers.Helper;
using Application.Domain.Enums;
using Application.Domain.Entity;
using Application.ResultType;

namespace Application.Customers.CustomerCommand
{
    public sealed record CreateCustomerCommand(CreateCustomerDTO CustomerDTO) : IRequest<ResultType.Result>;

    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IHasher _hasher;

        public CreateCustomerCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper, IMediator mediator, IHasher hasher)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _hasher = hasher;
        }
        public async Task<ResultType.Result> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            CustomerHelper helper = new CustomerHelper(); 

            //Create Customer
            var existingCustomer = await _context.Customers.AnyAsync(x => x.Email == request.CustomerDTO.Email);
            if (existingCustomer)
            {
                return ResultType.Result.Failure<CreateCustomerCommand>("Customer already exists");
            }
            //Hashing Password
            var (hashedPassword, salt)=_hasher.HashPassword(request.CustomerDTO.Password);

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
                Status = Status.Active
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
            ResultType.Result response = await _mediator.Send(new CreateAccountCommand(newAccount), cancellationToken);
            var responses = new
            {
                CustomerDTO = customerDTO,
                Data = response.Entity
            };
            return ResultType.Result.Success<CreateCustomerCommand>("Customer created successfully", responses);
        }
    }
}
