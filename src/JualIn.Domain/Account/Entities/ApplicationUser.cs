using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Account.Events;
using JualIn.Domain.Account.ValueObjects;
using JualIn.Domain.Common.Entities;

namespace JualIn.Domain.Account.Entities
{
    [Table("User")]
    public class ApplicationUser : RecoverableEntity
    {
        public string? IdentityId { get; set; }

        [StringLength(maximumLength: 32, MinimumLength = 8)]
        public required string Username { get; set; }

        [StringLength(256)]
        public required string Email { get; set; }

        [StringLength(64)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(64)]
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [StringLength(32)]
        public string? PhoneNumber { get; set; }

        public bool IsVerified { get; set; } = false;

        public byte[]? ProfilePicture { get; set; }

        [StringLength(16)]
        public required UserRole Role { get; set; }

        public long? OwnerId { get; set; }

        public void Register()
        {
            AddDomainEvent(new UserRegisteredEvent(Id, Username, Email));
        }
    }
}
