using JualIn.Domain.Inventories.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions
{
    public interface IStockMovementRepository : IReadWriteRepository<StockMovement>;
}
