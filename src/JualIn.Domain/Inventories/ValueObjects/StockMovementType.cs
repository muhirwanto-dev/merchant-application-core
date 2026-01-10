using Ardalis.SmartEnum;
using CommunityToolkit.Diagnostics;

namespace JualIn.Domain.Inventories.ValueObjects
{
    public class StockMovementType(string name, int value)
        : SmartEnum<StockMovementType>(name, value)
    {
        public static readonly StockMovementType In = new(nameof(In), 0);
        public static readonly StockMovementType Out = new(nameof(Out), 1);

        public static StockMovementType FromChangeValue(int stockChangeValue)
        {
            if (stockChangeValue > 0)
                return In;
            if (stockChangeValue < 0)
                return Out;

            return ThrowHelper.ThrowInvalidOperationException<StockMovementType>();
        }
    }
}
