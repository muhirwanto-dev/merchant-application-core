using CommunityToolkit.Mvvm.Messaging;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Services;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Sales.Messages;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Sales.Events;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.EventHandlers
{
    public sealed class OrderConfirmedEventHandler(
        IReportingService _reporting,
        IInventoryService _inventoryService,
        IProductService _productService,
        IMessenger _messenger
        ) : IDomainEventHandler<OrderConfirmedEvent>
    {
        public async Task Handle(OrderConfirmedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                await _inventoryService.UpdateStockAsync(domainEvent.OrderId, cancellationToken);
                await _productService.UpdateStockAsync(domainEvent.OrderId, cancellationToken);

                _messenger.Send(new OrderConfirmedMessage(domainEvent.OrderId, domainEvent.PaymentMethod));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
