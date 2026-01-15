using JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public partial class Mapper
    {
        private static OrderItem MapToModel(SaleProductViewModel source) => new()
        {
            Product = source.Entity,
            ProductId = source.Entity.Id,
            Quantity = source.OnCartQuantity,
            UnitPrice = source.Entity.Price,
            UnitNetPrice = source.Entity.Price,
            FinalUnitPrice = source.Entity.Price,
            TotalPrice = source.OnCartTotalPrice,
        };
    }
}
