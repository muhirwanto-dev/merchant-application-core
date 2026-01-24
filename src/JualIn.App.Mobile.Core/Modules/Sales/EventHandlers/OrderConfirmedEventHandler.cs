using JualIn.App.Mobile.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Core.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Sales.Events;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Core.Modules.Sales.EventHandlers
{
    public sealed class OrderConfirmedEventHandler(
        IReportingService _reporting,
        IInventoryService _inventoryService,
        IProductService _productService
        ) : IDomainEventHandler<OrderConfirmedEvent>
    {
        public async Task Handle(OrderConfirmedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                await _inventoryService.UpdateStockAsync(domainEvent.OrderId, cancellationToken);
                await _productService.UpdateStockAsync(domainEvent.OrderId, cancellationToken);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
