using Domain.Domain.Entity;
using Domain.Domain.Enums;
using Domain.Enums;

namespace Mini_Core_Banking_Project.Test.Generate;

public static class FakeAccount
{
    public static List<Account> GenerateFakeInactiveAccount()
    {
        List<Account> accountList =
        [
            new Account
            {
                Id = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b"),
                AccountNumber = "7894621348",
                AccountType = AccountType.Savings.ToString(),
                Balance = 50000,
                ClosedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                UpdatedAt = DateTime.Now,
                Status = Status.Inactive.ToString()
            },
            new Account
            {
                Id = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b"),
                AccountNumber = "6894628348",
                AccountType = AccountType.Savings.ToString(),
                Balance = 50000,
                ClosedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomerId = Guid.Parse("c0396c99-efeb-450e-bf1a-2ee1f8254773"),
                UpdatedAt = DateTime.Now,
                Status = Status.Inactive.ToString()
            },
        ];
        return accountList;
    }
    public static List<Account> GenerateFakeActiveAccount()
    {
        List<Account> accountList =
        [
            new Account
            {
                Id = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b"),
                AccountNumber = "7894621348",
                AccountType = AccountType.Savings.ToString(),
                Balance = 50000,
                ClosedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                UpdatedAt = DateTime.Now,
                Status = Status.Active.ToString()
            },
            new Account
            {
                Id = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b"),
                AccountNumber = "6894628348",
                AccountType = AccountType.Savings.ToString(),
                Balance = 50000,
                ClosedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CustomerId = Guid.Parse("c0396c99-efeb-450e-bf1a-2ee1f8254773"),
                UpdatedAt = DateTime.Now,
                Status = Status.Active.ToString()
            },
        ];
        return accountList;
    }
}
