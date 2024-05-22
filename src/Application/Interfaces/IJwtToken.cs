using Application.Domain.Entity;

namespace Application.Interfaces
{
    public interface IJwtToken
    {
        public string GenerateJWTToken(Customer customer);
        public string GenerateRefreshToken();
    }
}
