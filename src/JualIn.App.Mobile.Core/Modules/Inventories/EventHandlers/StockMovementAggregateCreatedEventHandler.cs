using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Core.Modules.Inventories.Events;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Inventories.Entities;
using Mediator;
using SingleScope.Persistence.Specification;

namespace JualIn.App.Mobile.Core.Modules.Inventories.EventHandlers
{
    public sealed class StockMovementAggregateCreatedEventHandler(
        IDomainEventDispatcher _domainEventDispatcher,
        IInventoryRepository _inventoryRepository,
        IStockMovementRepository _stockMovementRepository
        ) : INotificationHandler<StockMovementAggregateCreatedEvent>
    {
        public async ValueTask Handle(StockMovementAggregateCreatedEvent notification, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            foreach (var id in notification.Ids)
            {
                var sm = await _stockMovementRepository.FirstOrDefaultAsync(
                    new IncludeSpecification<StockMovement>(
                        criteria: x => x.Id == id,
                        include: x => x.Inventory)
                    , cancellationToken)
                    ?? ThrowHelper.ThrowArgumentNullException<StockMovement>();

                if (sm.Inventory == null)
                {
                    continue;
                }

                sm.Inventory.ApplyMovement(sm);

                await _inventoryRepository.UpdateAsync(sm.Inventory, cancellationToken);
                await _domainEventDispatcher.DispatchAsync(sm.Inventory.ConsumeEvents(), cancellationToken);
            }

            await _inventoryRepository.SaveAsync(cancellationToken);
        }
    }
}
