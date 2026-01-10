using Ardalis.SmartEnum;

namespace JualIn.Domain.Catalogs.ValueObjects
{
    public class ProductCategory(string name, string value)
        : SmartEnum<ProductCategory, string>(name, value)
    {
        public static readonly ProductCategory Food = new(nameof(Food), "Food");
        public static readonly ProductCategory Drink = new(nameof(Drink), "Drink");
        public static readonly ProductCategory Snack = new(nameof(Snack), "Snack");
    }
}
