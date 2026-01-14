using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using JualIn.Domain.Inventories.Entities;
using JualIn.Domain.Inventories.ValueObjects;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    public partial class Mapper
    {
        private static void MapToForm(InventoryViewModel source, InventoryFormViewModel target)
            => MapToForm(source.Entity, target);

        private static void MapToForm(Inventory source, InventoryFormViewModel target)
        {
            target.BatchNumber = source.BatchNumber;
            target.Category = source.Category;
            target.Description = source.Description;
            target.ExpirationDate = source.ExpirationDate;
            target.Image = source.Image;
            target.Id = source.Id;
            target.InventoryId = source.InventoryId;
            target.LastStockUpdate = source.LastStockUpdate;
            target.Name = source.Name;
            target.PurchaseDate = source.PurchaseDate;
            target.Sku = source.Sku;
            target.Stock = source.Stock.Value;
            target.StockThreshold = source.StockThreshold;
            target.StockUnit = source.StockUnit;
            target.SupplierId = source.SupplierId;
            target.SupplierName = source.SupplierName;
            target.UnitPrice = source.UnitPrice;
            target.Upc = source.Upc;
        }

        private static Inventory MapToDataModel(InventoryFormViewModel source) => new()
        {
            Category = source.Category,
            Name = source.Name,
            UnitPrice = source.UnitPrice,
            StockUnit = StockUnit.FromValue(source.StockUnit),
            BatchNumber = source.BatchNumber,
            Description = source.Description,
            Image = source.Image,
            Sku = source.Sku,
            Stock = new(source.Stock),
            StockThreshold = source.StockThreshold,
            SupplierId = source.SupplierId,
            SupplierName = source.SupplierName,
            Upc = source.Upc,
            ExpirationDate = source.ExpirationDate,
            LastStockUpdate = source.LastStockUpdate,
            PurchaseDate = source.PurchaseDate,
            Id = source.Id
        };

        private static InventoryViewModel MapFromDataModel(Inventory source) => new(source);
    }
}
