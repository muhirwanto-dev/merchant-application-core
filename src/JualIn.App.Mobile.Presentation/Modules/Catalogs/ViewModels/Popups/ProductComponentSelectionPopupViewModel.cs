using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using JualIn.App.Mobile.Presentation.Shared.ViewModels;
using SingleScope.Mvvm.Abstractions;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels.Popups
{
    [QueryProperty(nameof(Inventories), "inventories")]
    [QueryProperty(nameof(CurrentComponents), "components")]
    public partial class ProductComponentSelectionPopupViewModel(
        IReportingService _reporting,
        IPopupService _popupService
        ) : ObservableRecipient, IViewModel
    {
        [ObservableProperty]
        private IList<SelectableItemViewModel<InventoryViewModel>> _inventories = [];

        [ObservableProperty]
        private IList<ProductComponentViewModel> _currentComponents = [];

        [RelayCommand]
        private void Select(SelectableItemViewModel<InventoryViewModel> item)
        {
            try
            {
                item.IsSelected = !item.IsSelected;
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void ClearSelection()
        {
            try
            {
                foreach (var item in Inventories)
                {
                    item.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task DoneAsync()
        {
            try
            {
                var existing = CurrentComponents.Select(x => x.Inventory);
                var selected = Inventories.Where(x => x.IsSelected).Select(x => x.Item);
                var retained = existing.IntersectBy(selected.Select(x => x.Entity.Id), y => y.Id);
                var toAdd = selected.ExceptBy(existing.Select(x => x.Id), y => y.Entity.Id);
                var result = new List<ProductComponentViewModel>();

                foreach (var item in retained)
                {
                    if (CurrentComponents.FirstOrDefault(x => x.Inventory.Id == item.Id) is ProductComponentViewModel comp)
                    {
                        result.Add(comp);
                    }
                }

                foreach (var item in toAdd)
                {
                    result.Add(new ProductComponentViewModel(item.Entity));
                }

                await _popupService.ClosePopupAsync(Shell.Current, result);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
