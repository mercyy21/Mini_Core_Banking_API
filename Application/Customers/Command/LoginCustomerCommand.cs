using Application.Interfaces;
using Application.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.ResultType;

namespace Application.Customers.Command
{
    public sealed record LoginCustomerCommand(string Email,string Password): IRequest<Result>;

    public sealed class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, Result> 
    {
        private readonly IMiniCoreBankingDbContext _dbContext;
        private readonly IHasher _hasher;
        private readonly IJwtToken _jwtToken;
        public LoginCustomerCommandHandler(IMiniCoreBankingDbContext dbContext, IHasher hasher, IJwtToken jwtToken)
        {
            _dbContext = dbContext;
            _hasher = hasher;
            _jwtToken = jwtToken;
        }
        public async Task<Result> Handle(LoginCustomerCommand command, CancellationToken cancellationToken)
        {
            Customer existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x=> x.Email == command.Email);
            if (existingCustomer == null)
            {
             
                return Result.Failure<LoginCustomerCommand>("This email does not exist in our database");
            }
            bool correctPassword =_hasher.VerifyPassword(command.Password,existingCustomer.Password,existingCustomer.Salt);

            if ((correctPassword))
            {
                string tokenString = _jwtToken.GenerateJWTToken(existingCustomer);
                string refreshToken = _jwtToken.GenerateRefreshToken();
                RefreshToken storedRefreshToken = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    CustomerId = existingCustomer.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),

                };
                await _dbContext.RefreshTokens.AddAsync(storedRefreshToken);
                existingCustomer.IsLoggedIn = true;
                _dbContext.Customers.Update(existingCustomer);
                await _dbContext.SaveChangesAsync(cancellationToken);
                var responses = new
                {
                    TokenString = tokenString,
                    RefreshToken = refreshToken
                };
                return Result.Success<LoginCustomerCommand>("Login Successful", responses);
            }
            else
            {
                return Result.Failure("Wrong password, try again");
            }

        }
    }
}
