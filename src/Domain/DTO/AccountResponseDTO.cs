using Application.Domain.Enums;
using Application.Enums;

namespace Application.DTO
{
    public class AccountResponseDTO 
    {
        public Guid CustomerId { get; set; }
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public double Balance { get; set; }
        public Status Status { get; set; }

    }
}
