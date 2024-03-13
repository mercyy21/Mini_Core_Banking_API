using Domain.Enums;

namespace Domain.DTO
{
    public class AccountResponseDTO 
    {
        public Guid CustomerId { get; set; }
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public double Balance { get; set; }
        public string Status { get; set; }

    }
}
