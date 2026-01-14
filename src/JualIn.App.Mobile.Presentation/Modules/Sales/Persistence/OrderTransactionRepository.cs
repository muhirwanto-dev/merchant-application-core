using JualIn.App.Mobile.Data.Contexts;
using JualIn.App.Mobile.Presentation.Modules.Sales.Abstractions;
using JualIn.Domain.Sales.Entities;
using SingleScope.Persistence.EFCore.Repositories;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Persistence
{
    public class OrderTransactionRepository(AppDbContext context) : ReadWriteRepository<OrderTransaction, AppDbContext>(context),
        IOrderTransactionRepository;
}
