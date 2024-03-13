using Application.Accounts.Helper;
using AutoMapper;
using Domain.Domain.Enums;
using Domain.DTO;
using Domain.Entity;
using FluentValidation;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Accounts.AccountCommand
{

    public sealed record CreateAccountCommand(AccountDTO AccountDTO) : IRequest<ResponseModel>;

    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ResponseModel>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAccountCommand> _validator;
        public CreateAccountCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper, IValidator<CreateAccountCommand> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ResponseModel> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(command);
            //Create Account
            AccountHelper accountHelper = new AccountHelper(); //Response helper
            bool existingAccount = await _context.Accounts.AnyAsync(x => x.CustomerId == command.AccountDTO.CustomerId);
            if (existingAccount)
            {
                return new ResponseModel { Message = "Customer already has an account", Success = false };
            }
            bool customer = await _context.Customers.AnyAsync(x => x.Id == command.AccountDTO.CustomerId);
            if (!customer)
            {
                return new ResponseModel { Message = "Customer does not exist", Success = false };
            }
            Account newAccount = new Account
            {
                AccountNumber= GenerateAccountNumber(),
                CustomerId = command.AccountDTO.CustomerId,
                AccountType = command.AccountDTO.AccountType.ToString(),
                Balance = 0,
                CreatedAt = DateTime.Now,
                Status = Status.Active.ToString()
            };

            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync(cancellationToken);

            AccountResponseDTO accountDTO = _mapper.Map<AccountResponseDTO>(accountHelper.AccountResponse(newAccount));

            return new ResponseModel { Data = accountDTO, Message = "Account created successfully", Success = true };

        }
        public string GenerateAccountNumber()
        {
            Random random = new Random();
            const int length = 10; // Length of the account number

            StringBuilder accountNumber = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                accountNumber.Append(random.Next(10)); // Append a random digit (0-9)
            }

            return accountNumber.ToString();
        }
    }

}

