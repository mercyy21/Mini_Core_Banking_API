using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Domain.Domain.Entity
{
    public class Customer : Base
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public bool IsLoggedIn { get; set; }
        [Required]
        public DateTime LastLoggedIn { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public List<Transaction> TransactionHistory { get; set; }
    }
}
