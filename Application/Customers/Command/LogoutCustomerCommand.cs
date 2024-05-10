using Application.Domain.Entity;
using Application.ResultType;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Customers.Command
{
    public sealed record LogoutCustomerCommand() : IRequest<Result>;

    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCustomerCommand, Result>
    {
        public readonly IMiniCoreBankingDbContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public LogoutCommandHandler(IMiniCoreBankingDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(LogoutCustomerCommand command, CancellationToken cancellationToken)
        {
            string customerId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                // User ID not found in authentication context (unlikely scenario)
                return Result.Failure<LogoutCustomerCommand>( "Customer's ID not found");
            }
            Guid.TryParse(customerId, out var id);
            // Get existing Customer
            Customer exisitingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingCustomer == null)
            {
                return Result.Failure<LogoutCustomerCommand>("Customer does not exist.");
            }
            exisitingCustomer.IsLoggedIn = false;
            exisitingCustomer.LastLoggedIn= DateTime.UtcNow;
            _context.Customers.Update(exisitingCustomer);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<LogoutCustomerCommand>("Logout successful");


        }
    }
}

