using JualIn.Domain.Inventories.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Abstractions
{
    public interface IStockMovementRepository : IReadWriteRepository<StockMovement>;
}
