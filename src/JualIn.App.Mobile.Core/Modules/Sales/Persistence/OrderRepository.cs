using JualIn.App.Mobile.Core.Modules.Sales.Abstractions;
using JualIn.App.Mobile.Core.Persistence.Contexts;
using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Core.Modules.Sales.Persistence
{
    public class OrderRepository(AppDbContext context) : ReadWriteRepository<Order, AppDbContext>(context),
        IOrderRepository;
}
