using Domain.Domain.Entity;

namespace Application.Customers.Jwt
{
    public interface IJwtToken
    {
        public string GenerateJWTToken(Customer customer);
        public string GenerateRefreshToken();
    }
}
