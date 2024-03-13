using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DBContext
{
    public class MiniCoreBankingDbContext : DbContext, IMiniCoreBankingDbContext
    {
        public MiniCoreBankingDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }


    }
}
