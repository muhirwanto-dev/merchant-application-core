using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Inventories.Entities;

namespace JualIn.Domain.Catalogs.Entities
{
    [Table(nameof(ProductComponent))]
    public class ProductComponent : AuditableEntity
    {
        public long ProductId { get; set; }

        public long InventoryId { get; set; }

        public double QuantityPerUnit { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(InventoryId))]
        public Inventory? Inventory { get; set; }
    }
}
