using JualIn.Domain.Common;

namespace JualIn.Domain.Catalogs.Events
{
    public record ProductOutOfStockEvent(long ProductId) : IDomainEvent;
}
