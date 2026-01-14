using JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;
using JualIn.Domain.Catalogs.Entities;
using JualIn.SharedLib.Extensions;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public partial class Mapper
    {
        private void MapToForm(ProductViewModel source, ProductFormViewModel target)
            => MapToForm(source.Entity, target);

        private void MapToForm(Product source, ProductFormViewModel target)
        {
            target.Category = source.Category;
            target.Description = source.Description;
            target.Id = source.Id;
            target.Image = source.Image;
            target.Name = source.Name;
            target.Price = source.Price;
            target.Stock = source.Stock.Value;
            target.ProductComponents.AddRange(source.Components.Select(MapFromDataModel));
        }

        private Product MapFromForm(ProductFormViewModel source) => new()
        {
            Name = source.Name,
            Category = source.Category,
            Price = source.Price,
            CapitalPrice = source.CapitalPrice,
            Description = source.Description,
            Image = source.Image,
            Stock = new Domain.Common.ValueObjects.Stock<int>(source.Stock),
            Id = source.Id,
            Components = [.. source.ProductComponents.Select(MapToDataModel)],
        };

        private ProductComponentViewModel MapFromDataModel(ProductComponent source) => new(source.Inventory!)
        {
            QuantityPerUnit = source.QuantityPerUnit,
            ProductId = source.ProductId,
        };

        private ProductComponent MapToDataModel(ProductComponentViewModel source) => new()
        {
            ProductId = source.ProductId,
            InventoryId = source.Inventory!.Id,
            QuantityPerUnit = source.QuantityPerUnit
        };

        private static SaleProductViewModel MapToSaleProductViewModel(Product source) => new(source);

        private static ProductViewModel MapToProductViewModel(Product source) => new(source);
    }
}
