using Ardalis.SmartEnum;

namespace JualIn.Domain.Inventories.ValueObjects
{
    public class StockChangeReason(string name, string value)
        : SmartEnum<StockChangeReason, string>(name, value)
    {
        public static readonly StockChangeReason Sale = new(nameof(Sale), "Sale");
        public static readonly StockChangeReason Refund = new(nameof(Refund), "Refund");
        public static readonly StockChangeReason Adjustment = new(nameof(Adjustment), "Adjustment");
        public static readonly StockChangeReason Expired = new(nameof(Expired), "Expired");
        public static readonly StockChangeReason Opname = new(nameof(Opname), "Opname");
        public static readonly StockChangeReason Production = new(nameof(Production), "Production");
    }
}
