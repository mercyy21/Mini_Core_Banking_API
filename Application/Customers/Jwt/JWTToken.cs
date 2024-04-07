using Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Customers.Jwt
{
    public class JWTToken
    {
        public string GenerateJWTToken(Customer customer)
        {
            string? issuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["Issuer"];
            string? audience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["Audience"];
            string? key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["Key"];
            /*var claim = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, customer.Email),
                    new Claim(ClaimTypes.Name, customer.FirstName)
                };*/
            //Encrypt the secret key
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //Create the signature for the token
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            //Generate Securtiy Token
            var tokenOptions = new JwtSecurityToken(
                issuer,
                audience,
                null,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
