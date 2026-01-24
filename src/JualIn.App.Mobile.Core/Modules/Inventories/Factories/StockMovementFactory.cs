using JualIn.Domain.Inventories.Entities;
using JualIn.Domain.Inventories.ValueObjects;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Factories
{
    public class StockMovementFactory
    {
        public IEnumerable<StockMovement> Create(Order order) =>
            order.Items.SelectMany(item
                => item.Product?.Components.Select(component
                    => new StockMovement
                    {
                        OrderId = order.OrderId,
                        InventoryId = component.InventoryId,
                        Type = StockMovementType.Out,
                        Quantity = component.QuantityPerUnit * item.Quantity,
                        UserName = "system",
                        Reason = StockChangeReason.Sale,
                    }) ?? []);
    }
}
