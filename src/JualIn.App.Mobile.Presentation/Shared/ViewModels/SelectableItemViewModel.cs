using CommunityToolkit.Mvvm.ComponentModel;

namespace JualIn.App.Mobile.Presentation.Shared.ViewModels
{
    public partial class SelectableItemViewModel<T>(T @item)
        : ObservableObject
    {
        [ObservableProperty]
        private T _item = @item;

        [ObservableProperty]
        private bool _isSelected = false;
    }
}
