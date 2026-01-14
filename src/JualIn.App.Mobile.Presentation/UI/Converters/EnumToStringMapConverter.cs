using System.Globalization;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Maui.Converters;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    public abstract class EnumToStringMapConverter<TEnum> : EnumStringMapConverter<TEnum, TEnum>
        where TEnum : struct;

    public abstract class EnumStringMapConverter<TEnum, TUIType> : BaseConverter<TUIType, string>
        where TEnum : struct
    {
        public override string DefaultConvertReturnValue { get; set; } = string.Empty;

        public override TUIType DefaultConvertBackReturnValue { get; set; } = default!;

        public override string ConvertFrom(TUIType value, CultureInfo? culture)
        {
            if (typeof(TUIType) == typeof(string))
            {
                if (Enum.TryParse(value as string, out TEnum result))
                {
                    return MapToString(result);
                }
            }
            else if (typeof(TUIType) == typeof(TEnum))
            {
                return MapToString((TEnum)Convert.ChangeType(value, typeof(TEnum))!);
            }

            return ThrowHelper.ThrowInvalidOperationException<string>($"{value} is failed to parse as {typeof(TEnum)}");
        }

        public override TUIType ConvertBackTo(string value, CultureInfo? culture)
        {
            if (TryMapFromString(value, out TEnum result))
            {
                if (typeof(TUIType) == typeof(string))
                {
                    return (TUIType)Convert.ChangeType(value, typeof(TUIType));
                }
                else if (typeof(TUIType) == typeof(TEnum))
                {
                    return (TUIType)Convert.ChangeType(result, typeof(TUIType));
                }
            }

            return ThrowHelper.ThrowInvalidOperationException<TUIType>($"{value} is failed to parse as {typeof(TEnum)}");
        }

        protected abstract string MapToString(TEnum value);

        protected virtual bool TryMapFromString(string value, out TEnum @enum)
        {
            @enum = default;
            return ThrowHelper.ThrowNotSupportedException<bool>($"{nameof(DefaultConvertBackReturnValue)} is not used for ${nameof(BaseConverterOneWay<TUIType, string>)}");
        }
    }
}
