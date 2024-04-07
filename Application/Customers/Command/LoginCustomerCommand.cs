using Application.Customers.Jwt;
using Application.Customers.PasswordHasher;
using Domain.DTO;
using Domain.Entity;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Command
{
    public sealed record LoginCustomerCommand(string Email,string Password): IRequest<AuthenticatedResponse>;

    public sealed class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, AuthenticatedResponse> 
    {
        private readonly IMiniCoreBankingDbContext _dbContext;
        public LoginCustomerCommandHandler(IMiniCoreBankingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AuthenticatedResponse> Handle(LoginCustomerCommand command, CancellationToken cancellationToken)
        {
            Hasher hasher = new Hasher();
            Customer existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x=> x.Email == command.Email);
            if (existingCustomer == null)
            {
                return new AuthenticatedResponse { Message="This email does not exist on our database", Success=false };
            }
            bool correctPassword =hasher.VerifyPassword(command.Password,existingCustomer.Password,existingCustomer.Salt);

            if ((existingCustomer!=null)&& (correctPassword))
            {
                JWTToken token = new();
                string tokenString = token.GenerateJWTToken(existingCustomer);
                return new AuthenticatedResponse { Message = "Login Successful", Success = true, Token = tokenString };
            };
            
            return new AuthenticatedResponse { Message = "Error, try again", Success= false };

        }
    }
}
