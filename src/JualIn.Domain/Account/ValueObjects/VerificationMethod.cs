using Ardalis.SmartEnum;

namespace JualIn.Domain.Account.ValueObjects
{
    public class VerificationMethod(string name, string value)
        : SmartEnum<VerificationMethod, string>(name, value)
    {
        public static readonly VerificationMethod Email = new(nameof(Email), "Email");
        public static readonly VerificationMethod Phone = new(nameof(Phone), "Phone");

        public string ProviderName => Value;
    }
}
