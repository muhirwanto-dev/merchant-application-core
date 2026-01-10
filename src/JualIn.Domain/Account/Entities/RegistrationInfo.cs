using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Account.ValueObjects;
using JualIn.Domain.Common.Entities;

namespace JualIn.Domain.Account.Entities
{
    [Table(nameof(RegistrationInfo))]
    public class RegistrationInfo : AuditableEntity
    {
        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required string ConfirmedPasswordHash { get; set; }

        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? BusinessName { get; set; }

        public BusinessCategory? BusinessCategory { get; set; }
    }
}
