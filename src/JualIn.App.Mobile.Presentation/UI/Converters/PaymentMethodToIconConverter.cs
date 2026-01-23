using System.Globalization;
using CommunityToolkit.Maui.Converters;
using JualIn.Domain.Payments.ValueObjects;
using UraniumUI.Icons.MaterialSymbols;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    [AcceptEmptyServiceProvider]
    public class PaymentMethodToIconConverter : BaseConverterOneWay<string?, string?>
    {
        public override string? DefaultConvertReturnValue { get; set; } = null;

        public override string? ConvertFrom(string? value, CultureInfo? culture)
            => value is null ? null : MapToString(PaymentMethod.FromValue(value));

        protected static string MapToString(PaymentMethod value) => value.Name switch
        {
            nameof(PaymentMethod.Cash) => MaterialRounded.Money,
            nameof(PaymentMethod.CreditCard) => MaterialRounded.Credit_card,
            nameof(PaymentMethod.DebitCard) => MaterialRounded.Credit_card,
            nameof(PaymentMethod.EWallet) => MaterialRounded.Wallet,
            nameof(PaymentMethod.BankTransfer) => MaterialRounded.Account_balance,
            _ => throw new NotImplementedException(),
        };
    }
}
