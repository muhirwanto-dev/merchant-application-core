using JualIn.Domain.Catalogs.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Abstractions
{
    public interface IProductRepository : IReadWriteRepository<Product>
    {
        Task<List<Product>> GetProductsWithComponentAsync(CancellationToken cancellationToken = default);

        Task UpsertAsync(Product entity, CancellationToken cancellationToken = default);

        Task UpdateComponentsAsync(long productId, IEnumerable<ProductComponent> components, CancellationToken cancellationToken = default);
    }
}
