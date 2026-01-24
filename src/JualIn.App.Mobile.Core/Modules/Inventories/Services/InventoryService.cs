using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Core.Modules.Inventories.Models;
using JualIn.Domain.Sales.Entities;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Services
{
    public class InventoryService(
        IServiceProvider _serviceProvider,
        IReadRepository<Order> _orderRepository,
        IStockMovementRepository _stockMovementRepository,
        IPublisher _mediator
        ) : IInventoryService
    {
        public async Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken)
                ?? ThrowHelper.ThrowArgumentNullException<Order>($"Order with id: {orderId} should be exist!");
            var aggregate = _serviceProvider.GetRequiredService<StockMovementAggregate>();

            aggregate.Create(order);

            await _stockMovementRepository.AddRangeAsync(aggregate.StockMovements, cancellationToken);
            await _stockMovementRepository.SaveAsync(cancellationToken);
            await _mediator.Publish(aggregate.Created(), cancellationToken);
        }
    }
}
