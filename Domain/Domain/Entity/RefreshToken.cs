using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Domain.Entity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
