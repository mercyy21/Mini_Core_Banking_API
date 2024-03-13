using MediatR;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Accounts.AccountCommand;
using Application.Customers.Helper;
using Domain.Domain.Enums;
using FluentValidation;

namespace Application.Customers.CustomerCommand
{
    public sealed record CreateCustomerCommand(CreateCustomerDTO CustomerDTO) : IRequest<MultipleDataResponseModel>;

    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, MultipleDataResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IValidator<CreateCustomerCommand> _validator;

        public CreateCustomerCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper, IMediator mediator, IValidator<CreateCustomerCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<MultipleDataResponseModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            _validator.ValidateAndThrow(request);
            CustomerHelper helper = new CustomerHelper(); 

            //Create Customer
            var existingCustomer = await _context.Customers.AnyAsync(x => x.Email == request.CustomerDTO.Email);
            if (existingCustomer)
            {
                return new MultipleDataResponseModel { Message = "Customer already exists", Success = false };
            }
            Customer entity = new Customer
            {
                FirstName = request.CustomerDTO.FirstName,
                LastName = request.CustomerDTO.LastName,
                Email = request.CustomerDTO.Email,
                Address = request.CustomerDTO.Address,
                PhoneNumber = request.CustomerDTO.PhoneNumber,
                CreatedAt = DateTime.Now,
                Status = Status.Active.ToString()
            };

            //Save to list 
            await _context.Customers.AddAsync(entity, CancellationToken.None);
            await _context.SaveChangesAsync(CancellationToken.None);

            CustomerResponseDTO customerDTO = _mapper.Map<CustomerResponseDTO>(helper.GetCustomerResponse(request.CustomerDTO, entity.Id));

            //Create Account 
            AccountDTO newAccount = new AccountDTO
            {
                CustomerId = entity.Id,
                AccountType = request.CustomerDTO.AccountType
            };
            ResponseModel response = await _mediator.Send(new CreateAccountCommand(newAccount), CancellationToken.None);

            return new MultipleDataResponseModel { Data = new List<object> { customerDTO, response.Data }, 
                Message = "Customer created successfully", Success = true };
        }
    }
}
