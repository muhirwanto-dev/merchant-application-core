using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Common.ValueObjects;
using JualIn.Domain.Inventories.Events;
using JualIn.Domain.Inventories.ValueObjects;

namespace JualIn.Domain.Inventories.Entities
{
    [Table(nameof(Inventory))]
    public class Inventory : AuditableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(128)]
        public string InventoryId { get; } = default!;

        [StringLength(512)]
        public string Name { get; set; } = default!;

        public byte[]? Image { get; set; }

        [StringLength(32)]
        public string? Sku { get; set; }

        [StringLength(16)]
        public string? Upc { get; set; }

        public string? Description { get; set; }

        [StringLength(128)]
        public string Category { get; set; } = string.Empty;

        public long? SupplierId { get; set; }

        [StringLength(256)]
        public string? SupplierName { get; set; }

        public double UnitPrice { get; set; }

        public Stock<double> Stock { get; set; } = new(0);

        [StringLength(8)]
        public StockUnit StockUnit { get; set; } = StockUnit.Gram;

        public double StockThreshold { get; set; } = 0;

        public int? BatchNumber { get; set; }

        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        public DateTime LastStockUpdate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpirationDate { get; set; }

        public void ApplyMovement(StockMovement movement)
        {
            var now = DateTime.UtcNow;

            Guard.IsTrue(movement.InventoryId == Id);

            Stock.Update(movement.Type.Name switch
            {
                nameof(StockMovementType.In) => Stock.Value + movement.Quantity,
                nameof(StockMovementType.Out) => Stock.Value - movement.Quantity,
                _ => throw new InvalidOperationException()
            });
            LastStockUpdate = now;
            UpdatedAt = now;

            if (Stock.IsOutOfStock())
            {
                AddDomainEvent(new InventoryOutOfStockEvent(Id));
            }
            else if (Stock.IsLowStock(StockThreshold))
            {
                AddDomainEvent(new InventoryLowStockEvent(Id));
            }
        }
    }
}
