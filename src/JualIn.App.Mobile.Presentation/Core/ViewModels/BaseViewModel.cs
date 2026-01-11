using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using JualIn.App.Mobile.Presentation.Core.Messaging;
using JualIn.App.Mobile.Presentation.UI.Popups;
using SingleScope.Mvvm.Base;
using SingleScope.Mvvm.Maui;
using SingleScope.Navigations.Abstractions;
using SingleScope.Reporting.Abstractions;

namespace JualIn.App.Mobile.Presentation.Core.ViewModels
{
    public abstract class BaseViewModel : InteractiveViewModelBase
    {
        protected readonly IReportingService _reporting;
        protected readonly INavigationService _navigation;

        protected BaseViewModel()
        {
            _reporting = MauiServiceProvider.Current.GetRequiredService<IReportingService>();
            _navigation = MauiServiceProvider.Current.GetRequiredService<INavigationService>();
        }
    }
}
