using Ardalis.SmartEnum;

namespace JualIn.Domain.Account.ValueObjects
{
    public class UserRole(string name, int value)
        : SmartEnum<UserRole>(name, value)
    {
        public static readonly UserRole Owner = new(nameof(Owner), 0);
        public static readonly UserRole Cashier = new(nameof(Cashier), 1);
    }
}
