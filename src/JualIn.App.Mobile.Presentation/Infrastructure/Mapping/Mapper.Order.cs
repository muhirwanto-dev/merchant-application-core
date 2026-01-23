using JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;
using JualIn.Domain.Sales.Entities;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public partial class Mapper
    {
        private static OrderItemViewModel MapToViewModel(SaleProductViewModel source) => new(source.Entity)
        {
            Quantity = source.OnCartQuantity,
        };

        private static OrderItem MapToModel(OrderItemViewModel source) => new()
        {
            Product = source.Product,
            ProductId = source.Product.Id,
            Quantity = source.Quantity,
            UnitPrice = source.UnitPrice,
            UnitDiscount = source.UnitDiscount,
            UnitNetPrice = source.UnitNetPrice,
            UnitTax = source.UnitTax,
            FinalUnitPrice = source.FinalUnitPrice,
            TotalPrice = source.TotalPrice,
            Notes = source.Notes,
        };

        private static OrderItemViewModel MapToViewModel(OrderItem source) => new(source.Product!)
        {
            Quantity = source.Quantity,
            UnitDiscount = source.UnitDiscount,
            UnitTax = source.UnitTax,
            Notes = source.Notes,
        };
    }
}
