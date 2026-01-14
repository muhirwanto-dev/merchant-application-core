using System.Globalization;
using CommunityToolkit.Maui.Converters;
using JualIn.App.Mobile.Presentation.Resources.Strings;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    [AcceptEmptyServiceProvider]
    public class PaymentMethodToTextConverter : BaseConverterOneWay<string, string>
    {
        public override string DefaultConvertReturnValue { get; set; } = string.Empty;

        public override string ConvertFrom(string value, CultureInfo? culture)
            => Enum.TryParse<PaymentMethod>(value, out var method)
                ? MapToString(method)
                : DefaultConvertReturnValue;

        protected string MapToString(PaymentMethod value) => value switch
        {
            PaymentMethod.Cash => AppStrings.PaymentMethod_Lbl_Cash,
            //PaymentMethod.CreditCard => AppStrings.PaymentMethod_Lbl_CreditCard,
            //PaymentMethod.DebitCard => AppStrings.PaymentMethod_Lbl_DebitCard,
            //PaymentMethod.EWallet => AppStrings.PaymentMethod_Lbl_DebitCard,
            //PaymentMethod.BankTransfer => AppStrings.PaymentMethod_Lbl_BankTransfer,
            _ => throw new NotImplementedException(),
        };
    }
}
