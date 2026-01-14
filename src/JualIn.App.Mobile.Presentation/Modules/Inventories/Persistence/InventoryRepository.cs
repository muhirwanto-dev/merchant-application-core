using JualIn.App.Mobile.Data.Contexts;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Presentation.Shared.Persistence;
using JualIn.Domain.Inventories.Entities;
using Microsoft.EntityFrameworkCore;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Persistence
{
    public class InventoryRepository(AppDbContext context) : ReadWriteRepository<Inventory, AppDbContext>(context),
        IInventoryRepository, ICategoryResolver<Inventory>, ISearchable<Inventory>
    {
        public Task<string[]> GetCategoriesAsync(CancellationToken cancellationToken = default)
            => _set.AsNoTracking()
                .Select(x => x.Category)
                .Distinct()
                .ToArrayAsync(cancellationToken);

        public Task<List<Inventory>> SearchAsync(string query, CancellationToken cancellationToken = default)
            => string.IsNullOrEmpty(query)
                ? GetAllAsync(cancellationToken)
                : WhereAsync(x => EF.Functions.Like(x.Name, query), cancellationToken);

        public async Task UpsertAsync(Inventory entity, CancellationToken cancellationToken = default)
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
