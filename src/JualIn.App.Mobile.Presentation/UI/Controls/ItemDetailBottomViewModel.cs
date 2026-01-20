using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;
using SingleScope.Mvvm.Abstractions;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.UI.Controls
{
    public partial class ItemDetailBottomViewModel(
        IReportingService _reporting
        ) : ObservableObject, IViewModel, INavigationAware
    {
        [ObservableProperty]
        private Func<object, Task> _onEditItem = static _ => Task.CompletedTask;

        [ObservableProperty]
        private Func<object, Task> _onDeleteItem = static _ => Task.CompletedTask;

        [ObservableProperty]
        private object _item = default!;

        [RelayCommand]
        private async Task EditItemAsync(object item)
        {
            try
            {
                await OnEditItem.Invoke(item);
            }
            catch (Exception ex)
            {
                await _reporting.ReportAsync(ex);
            }
        }

        [RelayCommand]
        private async Task DeleteItemAsync(object item)
        {
            try
            {
                await OnDeleteItem.Invoke(item);
            }
            catch (Exception ex)
            {
                await _reporting.ReportAsync(ex);
            }
        }

        public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
        {
            if (parameters.TryGetValue("item", out var item))
            {
                Item = item;
            }

            if (parameters.TryGetValue("onEdit", out var onEdit))
            {
                OnEditItem = (Func<object, Task>)onEdit;
            }

            if (parameters.TryGetValue("onDelete", out var onDelete))
            {
                OnDeleteItem = (Func<object, Task>)onDelete;
            }
        }
    }
}
