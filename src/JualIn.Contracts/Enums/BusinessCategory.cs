using SingleScope.Common.Attributes;

namespace JualIn.Contracts.Enums
{
    [EnumStringMap]
    public enum BusinessCategory
    {
        Restaurant,
        Cafe,
        Bar,
        Bakery,
        Catering,
        [EnumStringName("Grocery Store")] GroceryStore,
        Supermarket,
    }
}
