using Ardalis.SmartEnum;

namespace JualIn.Domain.Sales.ValueObjects
{
    public class TransactionType(string name, int value)
        : SmartEnum<TransactionType>(name, value)
    {
        public static readonly TransactionType Sale = new(nameof(Sale), 0);
        public static readonly TransactionType Refund = new(nameof(Refund), 1);
        public static readonly TransactionType Adjustment = new(nameof(Adjustment), 2);
    }
}
