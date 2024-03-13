using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
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
        public DateTime? UpdatedAt { get; set; }
    }
}
