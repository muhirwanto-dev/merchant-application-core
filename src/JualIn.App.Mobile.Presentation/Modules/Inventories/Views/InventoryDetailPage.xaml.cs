using JualIn.App.Mobile.Presentation.Core.Behaviors;
using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Views;

[ViewModelOwner<InventoryDetailViewModel>(IsDefaultConstructor = true)]
public partial class InventoryDetailPage : ContentPage
{
    private static readonly NoStatusBarBehavior _noStatusBarBehavior = new();

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

#if IOS
        if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
#endif
        {
            _noStatusBarBehavior.Apply();
        }
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        _noStatusBarBehavior.Revert();
    }
}