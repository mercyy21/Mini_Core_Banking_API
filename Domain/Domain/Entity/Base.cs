using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
    }
}
