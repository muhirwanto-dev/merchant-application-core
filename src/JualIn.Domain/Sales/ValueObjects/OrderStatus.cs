using Ardalis.SmartEnum;
using CommunityToolkit.Diagnostics;

namespace JualIn.Domain.Sales.ValueObjects
{
    /// <summary>
    /// State transitions:
    /// 
    /// Draft → Confirmed
    /// Draft → Cancelled
    /// Confirmed → (no backward change)
    /// </summary>
    public class OrderStatus(string name, int value)
        : SmartEnum<OrderStatus>(name, value)
    {
        public static readonly OrderStatus Draft = new(nameof(Draft), 0);
        public static readonly OrderStatus Confirmed = new(nameof(Confirmed), 1);
        public static readonly OrderStatus Canceled = new(nameof(Canceled), 2);

        public OrderStatus Confirm()
        {
            Guard.IsTrue(this == Draft);

            return Confirmed;
        }

        public OrderStatus Cancel()
        {
            Guard.IsTrue(this == Draft);

            return Canceled;
        }
    }
}
