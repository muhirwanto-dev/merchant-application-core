using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryExiprationEvent(long InventoryId) : IDomainEvent;
}
