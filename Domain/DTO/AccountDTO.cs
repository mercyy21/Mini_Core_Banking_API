using Domain.Enums;

namespace Domain.DTO
{
    public class AccountDTO
    {
        public Guid CustomerId { get; set; }
        public AccountType AccountType { get; set; }
    }

}
