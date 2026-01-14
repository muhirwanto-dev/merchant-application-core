using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryLowStockEvent(long InventoryId) : IDomainEvent;
}
