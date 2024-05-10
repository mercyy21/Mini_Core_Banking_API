using Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Entity
{
    public class Account : Base
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string? AccountNumber { get; set; }
        [Required]
        public AccountType AccountType { get; set; }
        public double Balance { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
