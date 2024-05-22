using Application.Interfaces;
using Application.Domain.Entity;
using Application.DTO;
using MediatR;
using Application.ResultType;

namespace Application.RefreshTokens
{
    public sealed record RefreshAccessTokenCommand(string refreshToken): IRequest<Result>;

    public sealed class RefreshTokenHandler : IRequestHandler<RefreshAccessTokenCommand,Result>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IJwtToken _jwtToken;

        public RefreshTokenHandler(IMiniCoreBankingDbContext context, IJwtToken jwtToken)
        {
            _context = context;
            _jwtToken = jwtToken;
        }
        public async Task<Result> Handle(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
        {
           RefreshToken storedToken = _context.RefreshTokens.FirstOrDefault(x=> x.Token== command.refreshToken);
            if (storedToken == null)
            {
                return Result.Failure<RefreshAccessTokenCommand>("Invalid Token");
            }
            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Result.Failure<RefreshAccessTokenCommand>("Refresh token has expired");
            }
            Customer existingCustomer= _context.Customers.FirstOrDefault(x=> x.Id== storedToken.CustomerId);
            string accessToken = _jwtToken.GenerateJWTToken(existingCustomer);
            var response = new
            {
                AccessToken = accessToken
            };
            return  Result.Success<RefreshAccessTokenCommand>( "Access Token Generated Successfully", response);



        }
    }
}
