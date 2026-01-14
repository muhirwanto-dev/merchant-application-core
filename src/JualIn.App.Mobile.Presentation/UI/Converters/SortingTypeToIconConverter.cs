using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Shared.Enums;
using UraniumUI.Icons.MaterialSymbols;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Converters
{
    [AcceptEmptyServiceProvider]
    public class SortingTypeToIconConverter : EnumToStringMapConverter<SortingType>
    {
        protected override string MapToString(SortingType value) => value switch
        {
            SortingType.Disabled => MaterialRounded.Sort,
            SortingType.Ascending => MaterialRounded.Sort_by_alpha,
            _ => ThrowHelper.ThrowInvalidOperationException<string>($"{value} is failed to parse as icon source")
        };
    }
}
