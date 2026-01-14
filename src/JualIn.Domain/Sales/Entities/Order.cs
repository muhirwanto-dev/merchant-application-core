using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Diagnostics;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Payments.ValueObjects;
using JualIn.Domain.Sales.Events;
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

        public static Order Create(IEnumerable<OrderItem> items)
        {
            return new Order
            {
                Items = [.. items]
            };
        }

        /// <summary>
        /// todo: apply discount with voucher Domain
        /// </summary>
        /// <param name="discount"></param>
        public void ApplyDiscount(double discount)
        {
            Guard.IsGreaterThanOrEqualTo(discount, 0, nameof(discount));
            Guard.IsLessThanOrEqualTo(discount, TotalItemPrice, nameof(discount));

            TotalDiscount = discount;
        }

        public void Pay(double amount, PaymentMethod paymentMethod)
        {
            AddDomainEvent(new OrderPaidEvent(OrderId, amount, paymentMethod));
        }

        public void Confirm(PaymentMethod paymentMethod)
        {
            Guard.IsGreaterThan(Items.Count, 0, nameof(Items));

            AddDomainEvent(new OrderConfirmedEvent(OrderId, paymentMethod));
        }

        public void RemoveItem(long productId)
        {
            var item = Items.FirstOrDefault(i => i.Product?.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void AddItemQuantity(long productId, double quantity)
        {
            var item = Items.FirstOrDefault(i => i.Product?.Id == productId);
            if (item != null)
            {
                double newQuantity = quantity + item.Quantity;

                Guard.IsLessThanOrEqualTo(newQuantity, item.Product!.Stock.Value, nameof(quantity));
                Guard.IsGreaterThanOrEqualTo(newQuantity, 0, nameof(quantity));

                item.Quantity = newQuantity;
            }
        }
    }
}
