using JualIn.App.Mobile.Core.Modules.Sales.Abstractions;
using JualIn.App.Mobile.Core.Persistence.Contexts;
using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.EFCore.UnitOfWork;

namespace JualIn.App.Mobile.Core.Modules.Sales.Persistence
{
    public class OrderUnitOfWork(AppDbContext dbContext) : UnitOfWork<AppDbContext>(dbContext), IOrderUnitOfWork
    {
        public Task CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
            => ExecuteAsync(async _ =>
            {
                order.Items = [];
                order.Transactions = [];

                await _context.AddAsync(order, cancellationToken);
                await SaveChangesAsync(cancellationToken);

                // create order items
                foreach (var item in order.Items)
                {
                    item.OrderId = order.Id;

                    await _context.AddAsync(item, cancellationToken);
                }
            }, cancellationToken);
    }
}
