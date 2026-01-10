using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JualIn.Domain.Common.Entities;

namespace JualIn.Domain.Sales.Entities
{
    [Table(nameof(OrderTransaction))]
    public class OrderTransaction : AuditableEntity
    {
        [StringLength(128)]
        public string TransactionId { get; set; } = string.Empty;

        [StringLength(64)]
        public string TransactionType { get; set; } = string.Empty;

        public long OrderId { get; set; }

        public bool IsConfirmed { get; set; }

        /// <summary>
        /// Actual amount applied to the order total.
        /// Can be less than PaidAmount (cash change)
        /// Can be negative (refund)
        /// </summary>
        public double SettledAmount { get; set; }

        /// <summary>
        /// What customer paid (cash given, QR charged, etc.)
        /// Always POSITIVE
        /// </summary>
        public double PaidAmount { get; set; }

        /// <summary>
        /// PaidAmount - SettledAmount (cash change)
        /// </summary>
        public double ChangeAmount { get; set; }

        [StringLength(64)]
        public string PaymentMethod { get; set; } = string.Empty;

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
    }
}
