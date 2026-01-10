using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Sales.ValueObjects;

namespace JualIn.Domain.Sales.Entities
{
    [Table(nameof(Order))]
    public class Order : AuditableEntity
    {
        [StringLength(128)]
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(64)]
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        // sum of item net
        public double TotalItemPrice { get; set; }

        // item + order-level
        public double TotalDiscount { get; set; }

        public double TotalTax { get; set; }

        // final amount
        public double GrandTotal { get; set; }

        public ICollection<OrderItem> Items { get; set; } = [];

        public ICollection<OrderTransaction> Transactions { get; set; } = [];
    }
}
