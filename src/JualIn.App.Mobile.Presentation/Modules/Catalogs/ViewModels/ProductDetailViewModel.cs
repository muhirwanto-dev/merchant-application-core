using CommunityToolkit.Diagnostics;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Mapping;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.App.Mobile.Presentation.UI.Controls;
using JualIn.App.Mobile.Presentation.UI.Controls.Popups;
using Plugin.Maui.BottomSheet.Navigation;
using SingleScope.Maui.Dialogs.Models;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels
{
    [QueryProperty(nameof(Product), "product")]
    public partial class ProductDetailViewModel : BaseViewModel
    {
        private readonly IMapper _mapper;
        private readonly IPopupService _popupService;
        private readonly IProductRepository _productRepository;
        private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

        private static Func<object, Task>? _bottomOnEditAction;
        private static Func<object, Task>? _bottomOnDeleteAction;

        [ObservableProperty]
        private ProductViewModel _product = default!;

        public ProductDetailViewModel(
            IMapper mapper,
            IPopupService popupService,
            IProductRepository productRepository,
            IBottomSheetNavigationService bottomSheetNavigationService)
        {
            _mapper = mapper;
            _popupService = popupService;
            _productRepository = productRepository;
            _bottomSheetNavigationService = bottomSheetNavigationService;
        }

        [RelayCommand]
        private void Appearing()
        {
            try
            {
                Guard.IsNotNull(Product);

                _bottomOnEditAction ??= item => EditItemCommand.ExecuteAsync((ProductViewModel)item);
                _bottomOnDeleteAction ??= item => DeleteItemCommand.ExecuteAsync((ProductViewModel)item);

                _bottomSheetNavigationService.NavigateToAsync<ItemDetailBottomViewModel>(nameof(ItemDetailBottom),
                    new BottomSheetNavigationParameters
                    {
                        ["item"] = Product,
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
        private async Task DeleteItemAsync(ProductViewModel item)
        {
            try
            {
                bool confirmed = await _dialogService.ShowAsync(Dialog.Confirmation(
                    AppStrings.ProductDetail_Msg_ConfirmDeleteProduct, AppStrings.ProductDetail_Lbl_DeleteProductDialogTitle));
                if (!confirmed)
                {
                    return;
                }

                using var _ = StartScopedNavigation();

                await _productRepository.DeleteAsync(item.Entity.Id);
                await _productRepository.SaveAsync();

                await Task.WhenAll([
                    Toast.Make(AppStrings.ProductDetail_Msg_ProductDeleted).Show(),
                    _navigation.BackAsync(),
                ]);
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanNavigate))]
        private async Task EditItemAsync(ProductViewModel item)
        {
            using var _ = StartScopedNavigation();

            try
            {
                await _navigation.NavigateToAsync<ProductFormPage>(ShellNavigationParams.Create(("product", item)));
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
                var image = Product.Entity.Image;
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
