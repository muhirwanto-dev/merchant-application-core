using JualIn.Domain.Common.Messaging;

namespace JualIn.Domain.Catalogs.Events
{
    public record ProductOutOfStockEvent(long ProductId) : IDomainEvent;
}
