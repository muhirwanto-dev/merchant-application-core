using Ardalis.SmartEnum;

namespace JualIn.Domain.Inventories.ValueObjects
{
    public class StockUnit(string name, string value)
        : SmartEnum<StockUnit, string>(name, value)
    {
        public static readonly StockUnit Gram = new(nameof(Gram), "gr");
        public static readonly StockUnit Kilogram = new(nameof(Kilogram), "kg");
        public static readonly StockUnit Liter = new(nameof(Liter), "lt");
        public static readonly StockUnit Milliliter = new(nameof(Milliliter), "ml");
        public static readonly StockUnit Piece = new(nameof(Piece), "pcs");
        public static readonly StockUnit Box = new(nameof(Box), "box");
    }
}
