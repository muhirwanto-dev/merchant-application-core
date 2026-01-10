using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Catalogs.Entities;
using JualIn.Domain.Common.Entities;

namespace JualIn.Domain.Sales.Entities
{
    [Table(nameof(OrderItem))]
    public class OrderItem : AuditableEntity
    {
        public long OrderId { get; set; }

        public long ProductId { get; set; }

        public double Quantity { get; set; }

        /// <summary>
        /// Base price per unit at time of sale (before discount & tax)
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// Discount applied per unit (absolute value, not %)
        /// </summary>
        public double UnitDiscount { get; set; }

        /// <summary>
        /// Price per unit after discount, before tax
        /// </summary>
        public double UnitNetPrice { get; set; }

        /// <summary>
        /// Tax amount per unit
        /// </summary>
        public double UnitTax { get; set; }

        /// <summary>
        /// Final unit price (net + tax)
        /// </summary>
        public double FinalUnitPrice { get; set; }

        /// <summary>
        /// FinalPrice × Quantity
        /// </summary>
        public double TotalPrice { get; set; }

        public string? Notes { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
    }
}
