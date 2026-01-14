using JualIn.App.Mobile.Presentation.Modules.Inventories.Events;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Factories;
using JualIn.Domain.Inventories.Entities;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Models
{
    public class StockMovementAggregate(
        StockMovementFactory _factory
        )
    {
        public IEnumerable<StockMovement> StockMovements { get; private set; } = [];

        public void Create(Order order)
        {
            StockMovements = _factory.Create(order);
        }

        public StockMovementAggregateCreatedEvent Created() => new(StockMovements.Select(x => x.Id));
    }
}
