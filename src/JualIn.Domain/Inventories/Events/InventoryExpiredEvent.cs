using JualIn.Domain.Common;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryExpiredEvent(long InventoryId) : IDomainEvent;
}
