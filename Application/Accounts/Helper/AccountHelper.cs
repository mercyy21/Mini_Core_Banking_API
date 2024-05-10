using Application.Domain.Entity;

namespace Application.Accounts.Helper
{
    internal class AccountHelper
    {
        public Account AccountResponse(Account account)
        {
            return new Account
            {
                Id = account.Id,
                CustomerId = account.CustomerId,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt,
                Status = account.Status
            };
        }
    }
}
