using System.ComponentModel.DataAnnotations;

namespace Domain.Domain.Entity
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public string? SendersAccountNumber { get; set; }
        [Required]
        public string TransactionType { get; set; }
        public string? ReceiversAccountNumber { get; set; }

        [Required]
        public double Amount { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Narration { get; set; }
        public Guid CustomerId { get; set; }


    }
}
