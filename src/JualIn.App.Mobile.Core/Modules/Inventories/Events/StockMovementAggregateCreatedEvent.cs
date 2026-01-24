using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Events
{
    public record StockMovementAggregateCreatedEvent(IEnumerable<long> Ids) : INotification;
}
