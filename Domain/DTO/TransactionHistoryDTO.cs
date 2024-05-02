using Domain.Domain.Entity;
using Domain.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class TransactionHistoryDTO
    {
        public TransactionType TransactionType { get; set; }
        public NarrationType NarrationType { get; set; }
        public string SendersAccountNumber {  get; set; }
        public string ReceiversAccountNumber { get; set; }
        public double Amount { get; set; }
        public DateTime? TransactAt { get; set; }
        public Guid CustomerId { get; set; }

    }
}
