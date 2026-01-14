using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryExpiredEvent(long InventoryId) : IDomainEvent;
}
