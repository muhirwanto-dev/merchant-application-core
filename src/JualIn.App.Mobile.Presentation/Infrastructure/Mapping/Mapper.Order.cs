using JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public partial class Mapper
    {
        private OrderItem MapToModel(SaleProductViewModel source) => new(source.Entity)
        {
            Quantity = source.OnCartQuantity,
        };
    }
}
