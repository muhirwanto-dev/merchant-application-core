using JualIn.Domain.Common;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryOutOfStockEvent(long InventoryId) : IDomainEvent;
}
