using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Inventories.ValueObjects;

namespace JualIn.Domain.Inventories.Entities
{
    [Table(nameof(StockMovement))]
    public class StockMovement : AuditableEntity
    {
        public required string OrderId { get; set; }

        public long InventoryId { get; set; }

        [StringLength(64)]
        public StockMovementType Type { get; set; } = StockMovementType.Out;

        public required double Quantity { get; set; }

        [StringLength(128)]
        public required string UserName { get; set; }

        [StringLength(64)]
        public StockChangeReason Reason { get; set; } = StockChangeReason.Sale;

        [ForeignKey(nameof(InventoryId))]
        [Required]
        public Inventory? Inventory { get; set; } = null!;
    }
}
