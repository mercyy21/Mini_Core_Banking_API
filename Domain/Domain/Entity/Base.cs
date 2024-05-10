using Application.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Entity
{
    public class Base
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Status Status { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
