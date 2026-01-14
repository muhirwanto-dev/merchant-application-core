using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Inventories.Events
{
    public record InventoryOutOfStockEvent(long InventoryId) : IDomainEvent;
}
