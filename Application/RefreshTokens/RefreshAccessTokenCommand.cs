using Application.Interfaces;
using Application.Domain.Entity;
using Application.DTO;
using MediatR;

namespace Application.RefreshTokens
{
    public sealed record RefreshAccessTokenCommand(string refreshToken): IRequest<AuthenticatedResponse>;

    public sealed class RefreshTokenHandler : IRequestHandler<RefreshAccessTokenCommand,AuthenticatedResponse>
    {
        private readonly IMiniCoreBankingDbContext _context;
        private readonly IJwtToken _jwtToken;

        public RefreshTokenHandler(IMiniCoreBankingDbContext context, IJwtToken jwtToken)
        {
            _context = context;
            _jwtToken = jwtToken;
        }
        public async Task<AuthenticatedResponse> Handle(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
        {
           RefreshToken storedToken = _context.RefreshTokens.FirstOrDefault(x=> x.Token== command.refreshToken);
            if (storedToken == null)
            {
                return new AuthenticatedResponse { Message = "Invalid Token", Success = false };
            }
            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return new AuthenticatedResponse { Message = "Refresh token has expired", Success = false };
            }
            Customer existingCustomer= _context.Customers.FirstOrDefault(x=> x.Id== storedToken.CustomerId);
            string accessToken = _jwtToken.GenerateJWTToken(existingCustomer);
            return new AuthenticatedResponse { Message = "Access Token Generated Successfully", Success= true, AccessToken=  accessToken };



        }
    }
}
