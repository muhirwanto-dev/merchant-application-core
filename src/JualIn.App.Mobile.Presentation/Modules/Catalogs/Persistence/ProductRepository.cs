using JualIn.App.Mobile.Data.Contexts;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.Domain.Catalogs.Entities;
using Microsoft.EntityFrameworkCore;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Persistence
{
    public class ProductRepository(AppDbContext context) : ReadWriteRepository<Product, AppDbContext>(context),
        IProductRepository, ISearchable<Product>
    {
        public Task<List<Product>> GetProductsWithComponentAsync(CancellationToken cancellationToken = default)
            => _set.AsNoTracking()
                .Include(x => x.Components)
                .ThenInclude(c => c.Inventory)
                .ToListAsync(cancellationToken);

        public Task<List<Product>> SearchAsync(string query, CancellationToken cancellationToken = default)
            => string.IsNullOrEmpty(query)
                ? GetAllAsync(cancellationToken)
                : WhereAsync(x => EF.Functions.Like(x.Name, query), cancellationToken);

        public async Task UpdateComponentsAsync(long productId, IEnumerable<ProductComponent> components, CancellationToken cancellationToken = default)
        {
            var linked = await _context.ProductComponents.AsNoTracking().Where(x => x.ProductId == productId).ToArrayAsync(cancellationToken);
            var toRemove = linked.Except(components).Select(x =>
            {
                x.Inventory = null;
                x.Product = null;
                return x;
            });
            var toAdd = components.Except(linked).Select(x =>
            {
                x.Inventory = null;
                x.Product = null;
                return x;
            });

            _context.ProductComponents.RemoveRange(toRemove);
            _context.ProductComponents.AddRange(toAdd);
        }

        public async Task UpsertAsync(Product entity, CancellationToken cancellationToken = default)
        {
            var existing = await FindAsync(entity.Id, cancellationToken);
            if (existing == null)
            {
                await AddAsync(entity, cancellationToken);
            }
            else
            {
                DetatchFromTracking(entity);

                entity.CreatedAt = existing.CreatedAt;
                entity.UpdatedAt = DateTime.UtcNow;

                await UpdateAsync(entity, cancellationToken);
            }
        }
    }
}
