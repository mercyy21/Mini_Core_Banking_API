using Application.Customers.Jwt;
using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Customers.Command
{
   public sealed record LogoutCustomerCommand() : IRequest<ResponseModel>;

    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCustomerCommand, ResponseModel>
    {
        public readonly IMiniCoreBankingDbContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public LogoutCommandHandler(IMiniCoreBankingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> Handle(LogoutCustomerCommand command, CancellationToken cancellationToken)
        {
            string customerId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                // User ID not found in authentication context (unlikely scenario)
                return new ResponseModel { Message = "Customer's ID not found", Success = false };
            }
            Guid.TryParse(customerId, out var id);
            // Get existing Customer
            Customer exisitingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingCustomer == null)
            {
                return new ResponseModel { Message = "Customer does not exist.", Success=false };
            }
            exisitingCustomer.IsLoggedIn = false;
            exisitingCustomer.LastLoggedIn= DateTime.UtcNow;
            _context.Customers.Update(exisitingCustomer);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel { Message = "Logout successful", Success= true };


        }
    }
}

