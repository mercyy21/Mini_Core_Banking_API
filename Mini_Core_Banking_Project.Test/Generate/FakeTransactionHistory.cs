using Domain.Domain.Entity;
using Domain.Domain.Enums;

namespace Mini_Core_Banking_Project.Test.Generate;

public static class FakeTransactionHistory
{
    public static List<Transaction> GenerateFakeTransactionHistory()
    {
        return new List<Transaction>
        {
            new Transaction
            {
                Id= Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"),
                ReceiversAccountNumber="3457890123",
                SendersAccountNumber="6439012387",
                CustomerId= Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                Amount=2000,
                Narration= NarrationType.Transfer.ToString(),
                Timestamp= DateTime.Now,
                TransactionType= TransactionType.Debit.ToString()
            }
        };
    }
    public static List<Transaction> GenerateEmptyFakeTransactionHistory()
    {
        return new List<Transaction>
        {
            new Transaction
            {
            }
        };
    }
}
