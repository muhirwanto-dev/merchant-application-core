using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions
{
    public interface IOrderTransactionRepository : IReadWriteRepository<OrderTransaction>;
}
