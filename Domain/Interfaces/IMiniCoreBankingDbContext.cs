using Domain.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DBContext
{
    public interface IMiniCoreBankingDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
