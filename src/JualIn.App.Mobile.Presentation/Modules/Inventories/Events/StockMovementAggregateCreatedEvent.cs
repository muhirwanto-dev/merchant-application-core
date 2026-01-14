using Mediator;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Events
{
    public record StockMovementAggregateCreatedEvent(IEnumerable<long> Ids) : INotification;
}
