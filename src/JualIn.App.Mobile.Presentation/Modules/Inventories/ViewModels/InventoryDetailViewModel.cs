using CommunityToolkit.Diagnostics;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Inventories.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.App.Mobile.Presentation.UI.Controls;
using JualIn.App.Mobile.Presentation.UI.Controls.Popups;
using JualIn.Domain.Inventories.Entities;
using Plugin.Maui.BottomSheet.Navigation;
using SingleScope.Maui.Dialogs.Models;
using SingleScope.Navigations.Maui.Models;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels
{
    [QueryProperty(nameof(Inventory), "inventory")]
    public partial class InventoryDetailViewModel(
        IPopupService _popupService,
        IReadWriteRepository<Inventory> _inventoryRepository,
        IBottomSheetNavigationService _bottomSheetNavigationService
        ) : BaseViewModel
    {
        private static Func<object, Task>? _bottomOnEditAction;
        private static Func<object, Task>? _bottomOnDeleteAction;

        [ObservableProperty]
        private InventoryViewModel _inventory = default!;

        [RelayCommand]
        private void Appearing()
        {
            try
            {
                Guard.IsNotNull(Inventory);

                _bottomOnEditAction ??= item => EditItemCommand.ExecuteAsync((InventoryViewModel)item);
                _bottomOnDeleteAction ??= item => DeleteItemCommand.ExecuteAsync((InventoryViewModel)item);

                _bottomSheetNavigationService.NavigateToAsync<ItemDetailBottomViewModel>(nameof(ItemDetailBottom),
                    new BottomSheetNavigationParameters
                    {
                        ["item"] = Inventory,
                        ["onEdit"] = _bottomOnEditAction,
                        ["onDelete"] = _bottomOnDeleteAction,
                    });
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private void Disappearing()
        {
            try
            {
                _bottomSheetNavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task BackButtonAsync()
        {
            try
            {
                await _navigation.BackAsync();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task DeleteItemAsync(InventoryViewModel item)
        {
            try
            {
                bool confirmed = await _dialogService.ShowAsync(Confirmation.Untitled(AppStrings.InventoryDetail_Msg_ConfirmDeleteInventory, AppStrings.InventoryDetail_Lbl_DeleteInventoryDialogTitle));
                if (!confirmed)
                {
                    return;
                }

                using var _ = StartScopedNavigation();

                await _inventoryRepository.DeleteAsync(item.Entity.Id);
                await Task.WhenAll([
                    Toast.Make(AppStrings.InventoryDetail_Msg_InventoryDeleted).Show(),
                    _navigation.BackAsync(ShellNavigationParams.Create(("refresh", true))),
                ]);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task EditItemAsync(InventoryViewModel item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<InventoryFormPage>(ShellNavigationParams.Create(("inventory", item)));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task OpenFullScreenImageAsync()
        {
            try
            {
                var image = Inventory?.Entity.Image;
                if (image == null)
                {
                    return;
                }

                await _popupService.ShowPopupAsync<FullScreenImagePopupViewModel>(Shell.Current,
                    options: null,
                    shellParameters: new Dictionary<string, object>
                    {
                        ["image"] = image
                    });
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
