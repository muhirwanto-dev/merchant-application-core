using CommunityToolkit.Diagnostics;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Services
{
    public class ProductService(
        IOrderRepository _orderRepository,
        IProductRepository _productRepository
        ) : IProductService
    {
        public async Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var order = await _orderRepository.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken)
                ?? ThrowHelper.ThrowArgumentNullException<OrderDto>($"Order with id: {orderId} should be exist!");

            foreach (var item in order.Items)
            {
                var entity = await _productRepository.FindAsync(item.Product!.Id, cancellationToken);
                if (entity == null)
                {
                    continue;
                }

                _productRepository.DetatchFromTracking(entity);

                var product = new Product(entity);

                product.ApplyOrder(item);

                await _productRepository.UpdateAsync(product.Data, cancellationToken);
            }

            await _productRepository.SaveAsync(cancellationToken);
        }
    }
}
