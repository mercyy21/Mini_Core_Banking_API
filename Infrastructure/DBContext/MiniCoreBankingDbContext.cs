
using Application.Domain.Entity;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DBContext
{
    public class MiniCoreBankingDbContext : DbContext, IMiniCoreBankingDbContext
    {
        public MiniCoreBankingDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .Property(d => d.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Account>()
                         .Property(d => d.Status)
                         .HasConversion<string>();
            modelBuilder.Entity<Account>()
                         .Property(d => d.AccountType)
                         .HasConversion<string>();
            modelBuilder.Entity<Transaction>()
                        .Property(d => d.TransactionType)
                        .HasConversion<string>();
            modelBuilder.Entity<Transaction>()
                       .Property(d => d.Narration)
                       .HasConversion<string>();
        }


    }
}
