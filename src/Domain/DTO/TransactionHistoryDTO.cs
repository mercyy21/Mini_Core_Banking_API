using Application.Domain.Entity;
using Application.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class TransactionHistoryDTO
    {
        public TransactionType TransactionType { get; set; }
        public string? TransactionTypeDesc { get; set; }
        public NarrationType Narration { get; set; }
        public string? NarrationDesc { get; set; }
        public string? SendersAccountNumber {  get; set; }
        public string? ReceiversAccountNumber { get; set; }
        public double Amount { get; set; }
        public DateTime? TransactAt { get; set; }
        public Guid CustomerId { get; set; }

    }
}
