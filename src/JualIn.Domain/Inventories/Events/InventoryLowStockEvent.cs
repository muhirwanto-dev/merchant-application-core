using JualIn.Domain.Common;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryLowStockEvent(long InventoryId) : IDomainEvent;
}
