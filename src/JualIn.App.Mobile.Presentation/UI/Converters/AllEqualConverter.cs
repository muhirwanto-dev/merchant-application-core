using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    public class AllEqualConverter : ICommunityToolkitMultiValueConverter
    {
        public object? Convert(object[]? values, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (values == null || values.Length == 1)
            {
                return false;
            }

            for (int i = 1; i < values.Length; i++)
            {
                if (!Equals(values[0], values[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}
