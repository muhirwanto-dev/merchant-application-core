using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Services
{
    public class ProductService(
        IOrderRepository _orderRepository,
        IProductRepository _productRepository,
        IDomainEventDispatcher _dispatcher
        ) : IProductService
    {
        public async Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var order = await _orderRepository.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken)
                ?? ThrowHelper.ThrowArgumentNullException<Order>($"Order with id: {orderId} should be exist!");
            var events = new List<IDomainEvent>();

            foreach (var item in order.Items)
            {
                var entity = await _productRepository.FindAsync(item.Product!.Id, cancellationToken);
                if (entity == null)
                {
                    continue;
                }

                _productRepository.DetatchFromTracking(entity);

                entity.ApplyOrder(item);

                await _productRepository.UpdateAsync(entity, cancellationToken);

                events.AddRange(entity.ConsumeEvents());
            }

            await _productRepository.SaveAsync(cancellationToken);
            await _dispatcher.DispatchAsync(events, cancellationToken);
        }
    }
}
