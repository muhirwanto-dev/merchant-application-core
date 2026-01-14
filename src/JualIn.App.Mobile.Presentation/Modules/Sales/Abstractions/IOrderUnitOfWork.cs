using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions
{
    public interface IOrderUnitOfWork : IUnitOfWork
    {
        Task CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    }
}
