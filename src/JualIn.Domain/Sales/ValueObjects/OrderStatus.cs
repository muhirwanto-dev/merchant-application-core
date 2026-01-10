using Ardalis.SmartEnum;
using CommunityToolkit.Diagnostics;

namespace JualIn.Domain.Sales.ValueObjects
{
    /// <summary>
    /// State transitions:
    /// 
    /// Draft → Completed
    /// Draft → Cancelled
    /// Completed → (no backward change)
    /// </summary>
    public class OrderStatus(string name, int value)
        : SmartEnum<OrderStatus>(name, value)
    {
        public static readonly OrderStatus Draft = new(nameof(Draft), 0);
        public static readonly OrderStatus Completed = new(nameof(Completed), 1);
        public static readonly OrderStatus Canceled = new(nameof(Canceled), 2);

        public OrderStatus Next()
        {
            Guard.IsTrue(this == Draft);

            return Completed;
        }

        public OrderStatus Cancel()
        {
            Guard.IsTrue(this == Draft);

            return Canceled;
        }
    }
}
