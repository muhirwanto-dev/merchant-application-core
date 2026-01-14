using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Popups
{
    [QueryProperty(nameof(Image), "image")]
    public partial class FullScreenImagePopupViewModel(
        IReportingService _reporting,
        IPopupService _popupService
        ) : ObservableObject
    {
        [ObservableProperty]
        private byte[]? _image;

        [RelayCommand]
        private async Task CloseAsync()
        {
            try
            {
                await _popupService.ClosePopupAsync(Shell.Current);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
