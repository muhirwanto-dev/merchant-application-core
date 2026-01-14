using CommunityToolkit.Maui.Views;

namespace JualIn.App.Mobile.Presentation.UI.Controls.Popups;

[ViewModelOwner<FullScreenImagePopupViewModel>(IsDefaultConstructor = false)]
public partial class FullScreenImagePopup : Popup
{
    public FullScreenImagePopup()
    {
        InitializeComponent();
        PostInitializeComponent();

        Container.HeightRequest = DeviceDisplay.Current.MainDisplayInfo.Height;
        Container.WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width;
    }
}