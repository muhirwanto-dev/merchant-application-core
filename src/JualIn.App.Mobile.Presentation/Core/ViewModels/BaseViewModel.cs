
using SingleScope.Maui.Dialogs.Abstractions;
using SingleScope.Maui.Loadings.Abstractions;
using SingleScope.Mvvm.Base;
using SingleScope.Mvvm.Maui;
using SingleScope.Navigations.Abstractions;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.Core.ViewModels
{
    public abstract class BaseViewModel : InteractiveViewModelBase
    {
        protected readonly IDialogService _dialogService;
        protected readonly ILoadingService _loadingService;
        protected readonly INavigationService _navigation;
        protected readonly IReportingService _reporting;

        protected BaseViewModel()
        {
            _dialogService = MauiServiceProvider.Current.GetRequiredService<IDialogService>();
            _loadingService = MauiServiceProvider.Current.GetRequiredService<ILoadingService>();
            _navigation = MauiServiceProvider.Current.GetRequiredService<INavigationService>();
            _reporting = MauiServiceProvider.Current.GetRequiredService<IReportingService>();
        }
    }
}
