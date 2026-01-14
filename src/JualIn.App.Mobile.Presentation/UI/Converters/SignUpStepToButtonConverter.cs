using System.Globalization;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    public class SignUpStepToButtonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int stepIndex)
            {
                if (stepIndex == 0)
                {
                    return Resources.Strings.AppStrings.SignUpPage_Btn_Next;
                }
                else
                {
                    return Resources.Strings.AppStrings.SignUpPage_Btn_Register;
                }
            }

            return Resources.Strings.AppStrings.SignUpPage_Btn_Next;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
