using Application.Enums;

namespace Application.DTO
{
    public class AccountDTO
    {
        public Guid CustomerId { get; set; }
        public AccountType AccountType { get; set; }
    }

}
