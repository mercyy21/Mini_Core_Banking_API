using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Account : Base
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountType { get; set; }
        public double Balance { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
