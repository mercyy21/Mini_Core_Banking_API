using Application.Customers.Jwt;
using Application.Customers.PasswordHasher;
using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Command
{
    public sealed record LoginCustomerCommand(string Email,string Password): IRequest<LoginResponseDTO>;

    public sealed class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, LoginResponseDTO> 
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
        public async Task<LoginResponseDTO> Handle(LoginCustomerCommand command, CancellationToken cancellationToken)
        {
            Customer existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(x=> x.Email == command.Email);
            if (existingCustomer == null)
            {
                return new LoginResponseDTO { Message="This email does not exist in our database", Success=false };
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
                return new LoginResponseDTO { Message = "Login Successful", Success = true, AccessToken =tokenString,RefreshToken= refreshToken };
            }
            else
            {
                return new LoginResponseDTO { Message = "Wrong password, try again", Success = false };

            }

        }
    }
}
