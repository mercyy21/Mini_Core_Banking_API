using Application.Accounts.Helper;
using AutoMapper;
using Application.Domain.Entity;
using Application.Domain.Enums;
using Application.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Application.Interfaces;
using Application.ResultType;

namespace Application.Accounts.AccountCommand
{

    public sealed record CreateAccountCommand(AccountDTO AccountDTO) : IRequest<ResultType.Result>;

    public sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ResultType.Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IMapper _mapper;
        public CreateAccountCommandHandler(IMiniCoreBankingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResultType.Result> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            //Create Account
            AccountHelper accountHelper = new AccountHelper(); //Response helper
            bool existingAccount = await _context.Accounts.AnyAsync(x => x.CustomerId == command.AccountDTO.CustomerId);
            if (existingAccount)
            {
                return ResultType.Result.Failure<CreateAccountCommand>("Customer already has an account");
            }
            bool customer = await _context.Customers.AnyAsync(x => x.Id == command.AccountDTO.CustomerId);
            if (!customer)
            {
                return ResultType.Result.Failure<CreateAccountCommand>("Customer does not exist");
            }
            Account newAccount = new Account
            {
                AccountNumber= GenerateAccountNumber(),
                CustomerId = command.AccountDTO.CustomerId,
                AccountType = command.AccountDTO.AccountType,
                Balance = 0,
                CreatedAt = DateTime.Now,
                Status = Status.Active
            };

            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync(cancellationToken);

            AccountResponseDTO accountDTO = _mapper.Map<AccountResponseDTO>(accountHelper.AccountResponse(newAccount));

            return ResultType.Result.Success<CreateAccountCommand>("Account created successfully", accountDTO);

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

