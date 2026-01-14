using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Account.Events
{
    public record UserRegisteredEvent(long UserId, string Email, string Code) : IDomainEvent;
}
