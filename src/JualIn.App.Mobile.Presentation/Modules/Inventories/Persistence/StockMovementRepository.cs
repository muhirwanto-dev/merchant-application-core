using JualIn.App.Mobile.Data.Contexts;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions;
using JualIn.Domain.Inventories.Entities;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Persistence
{
    public class StockMovementRepository(AppDbContext context) : ReadWriteRepository<StockMovement, AppDbContext>(context),
        IStockMovementRepository;
}
