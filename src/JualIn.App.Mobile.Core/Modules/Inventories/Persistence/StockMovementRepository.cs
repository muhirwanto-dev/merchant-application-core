using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Core.Persistence.Contexts;
using JualIn.Domain.Inventories.Entities;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Persistence
{
    public class StockMovementRepository(AppDbContext context) : ReadWriteRepository<StockMovement, AppDbContext>(context),
        IStockMovementRepository;
}
