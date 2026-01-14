using System.Globalization;
using JualIn.App.Mobile.Presentation.Resources.Strings;

namespace JualIn.App.Mobile.Presentation.UI.Converters
{
    public class TimeToStateTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is not DateTime date)
            {
                return null;
            }

            int hour = date.Hour;
            if (hour >= 0 && hour < 12)
            {
                return AppStrings.DashboardPage_Lbl_Greeting_Morning;
            }
            else if (hour >= 12 && hour < 17)
            {
                return AppStrings.DashboardPage_Lbl_Greeting_Afternoon;
            }
            else if (hour >= 17 && hour < 21)
            {
                return AppStrings.DashboardPage_Lbl_Greeting_Evening;
            }
            else
            {
                return AppStrings.DashboardPage_Lbl_Greeting_Night;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
