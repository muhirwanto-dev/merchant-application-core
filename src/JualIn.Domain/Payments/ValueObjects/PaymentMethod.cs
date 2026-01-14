using Ardalis.SmartEnum;

namespace JualIn.Domain.Payments.ValueObjects
{
    public class PaymentMethod(string name, string value)
        : SmartEnum<PaymentMethod, string>(name, value)
    {
        public static readonly PaymentMethod Cash = new(nameof(Cash), "Cash");
        public static readonly PaymentMethod CreditCard = new(nameof(CreditCard), "Credit Card");
        public static readonly PaymentMethod DebitCard = new(nameof(DebitCard), "Debit Card");
        public static readonly PaymentMethod EWallet = new(nameof(EWallet), "e-Wallet");
        public static readonly PaymentMethod BankTransfer = new(nameof(BankTransfer), "Bank Transfer");
    }
}
