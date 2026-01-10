using JualIn.Application.Persistence.Repositories;
using JualIn.Domain.Inventories.Entities;
using JualIn.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.Infrastructure.Persistence.Repositories
{
    internal class InventoryRepository(AppDbContext dbContext) : ReadWriteRepository<Inventory, AppDbContext>(dbContext),
        ISearchableRepository<Inventory>
    {
        public Task<List<Inventory>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            return _set
                .AsNoTracking()
                .Where(x => EF.Functions.Like(x.Name, query))
                .ToListAsync(cancellationToken);
        }
    }
}
