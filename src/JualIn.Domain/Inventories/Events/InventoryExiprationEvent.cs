using JualIn.Domain.Common;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryExiprationEvent(long InventoryId) : IDomainEvent;
}
