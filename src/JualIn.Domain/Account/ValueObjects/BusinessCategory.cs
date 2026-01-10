using Ardalis.SmartEnum;

namespace JualIn.Domain.Account.ValueObjects
{
    public class BusinessCategory(string name, string value)
        : SmartEnum<BusinessCategory, string>(name, value)
    {
        public static readonly BusinessCategory Restaurant = new(nameof(Restaurant), "Restaurant");
        public static readonly BusinessCategory Cafe = new(nameof(Cafe), "Cafe");
        public static readonly BusinessCategory Bar = new(nameof(Bar), "Bar");
        public static readonly BusinessCategory Bakery = new(nameof(Bakery), "Bakery");
        public static readonly BusinessCategory Catering = new(nameof(Catering), "Catering");
        public static readonly BusinessCategory GroceryStore = new(nameof(GroceryStore), "Grocery Store");
        public static readonly BusinessCategory Supermarket = new(nameof(Supermarket), "Supermarket");
    }
}
