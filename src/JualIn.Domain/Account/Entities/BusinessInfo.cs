using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Account.ValueObjects;
using JualIn.Domain.Common.Entities;

namespace JualIn.Domain.Account.Entities
{
    [Table(nameof(BusinessInfo))]
    public class BusinessInfo : RecoverableEntity
    {
        public required long UserId { get; set; }

        [StringLength(128)]
        public string BusinessName { get; set; } = string.Empty;

        [StringLength(32)]
        public BusinessCategory? Category { get; set; }

        [ForeignKey(nameof(UserId))]
        [Required]
        public ApplicationUser? User { get; set; } = null!;
    }
}
