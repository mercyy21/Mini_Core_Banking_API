using Application.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Entity
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public string? SendersAccountNumber { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        public string? TransactionTypeDesc { get; set; }
        public string? ReceiversAccountNumber { get; set; }
        [Required]
        public double Amount { get; set; }
        public DateTime? Timestamp { get; set; }
        public NarrationType Narration { get; set; }
        public string? NarrationDesc { get; set; }
        public Guid CustomerId { get; set; }


    }
}
