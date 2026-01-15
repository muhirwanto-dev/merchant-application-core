using System.Globalization;
using CommunityToolkit.Maui.Converters;
using JualIn.Domain.Payments.ValueObjects;
using UraniumUI.Icons.MaterialSymbols;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    [AcceptEmptyServiceProvider]
    public class PaymentMethodToIconConverter : BaseConverterOneWay<string, string>
    {
        public override string DefaultConvertReturnValue { get; set; } = string.Empty;

        public override string ConvertFrom(string value, CultureInfo? culture)
            => MapToString(PaymentMethod.FromValue(value));

        protected static string MapToString(PaymentMethod value) => value.Name switch
        {
            nameof(PaymentMethod.Cash) => MaterialRounded.Money,
            //PaymentMethod.CreditCard => MaterialRounded.Credit_card,
            //PaymentMethod.DebitCard => MaterialRounded.Credit_card,
            //PaymentMethod.EWallet => MaterialRounded.Wallet,
            //PaymentMethod.BankTransfer => MaterialRounded.Account_balance,
            _ => throw new NotImplementedException(),
        };
    }
}
