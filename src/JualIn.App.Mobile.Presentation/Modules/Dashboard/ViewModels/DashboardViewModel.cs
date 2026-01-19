using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.Views;
using JualIn.App.Mobile.Presentation.Modules.Sales.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.SharedLib;
using SingleScope.Maui.Dialogs.Abstractions;

namespace JualIn.App.Mobile.Presentation.Modules.Dashboard.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private DateTime _clock = DateTime.Now;

        [ObservableProperty]
        private string _selectedRevenueText = AppStrings.DashboardPage_RevenuePreview_Filter_Revenue;

        [ObservableProperty]
        private float _revenueValue;

        [ObservableProperty]
        private float _revenueTarget;

        [ObservableProperty]
        private float _revenueProgress;

        [ObservableProperty]
        private DateTime _lastRevenueUpdate = DateTime.Now;

        public string FullName => _authService.User.FullName;

        public DashboardViewModel(IAuthService authService)
        {
            _authService = authService;

            var timer = new System.Timers.Timer(1000);

            timer.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(() => Clock = DateTime.Now);
            timer.Start();
        }

        [RelayCommand]
        private async Task AppearingAsync()
        {
            try
            {
                await using var _ = _loadingService.ShowAsync(3000);

                while (!_authService.HasUserData)
                {
                    await Waiting.One;
                }

                OnPropertyChanged(nameof(FullName));
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task OpenPointOfSalePageAsync()
        {
            try
            {
                await _navigation.NavigateToAsync<PointOfSalePage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task OpenProductManagementPageAsync()
        {
            try
            {
                await _navigation.NavigateToAsync<ProductManagementPage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task OpenOrderHistoryPageAsync()
        {
            try
            {
                await _navigation.NavigateToAsync<OrderHistoryPage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
