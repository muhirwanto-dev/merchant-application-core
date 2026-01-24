using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Sales.Abstractions
{
    public interface IOrderTransactionRepository : IReadWriteRepository<OrderTransaction>;
}
