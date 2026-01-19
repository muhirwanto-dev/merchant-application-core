using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Api;
using JualIn.App.Mobile.Presentation.Modules.Auth.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using SingleScope.Maui.Dialogs;
using SingleScope.Navigations.Maui.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.ViewModels
{
    public partial class SignUp_Step0_ViewModel : BaseViewModel
    {
        private readonly IBackendApi _api;

        [ObservableProperty] private string? _email;

        public SignUp_Step0_ViewModel(
            IBackendApi api)
        {
            _api = api;

            RegisterInteractionCommand(nameof(IsUserInteraction), SignInCommand);
        }

        [RelayCommand]
        private void Appearing()
        {
            try
            {
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand]
        private async Task NextAsync()
        {
            try
            {
                await using var _ = _loadingService.ShowAsync();
                var response = await _api.CheckAvailabilityAsync(Email!);

                if (response.IsSuccessful)
                {
                    if (!(response.Content?.IsAvailable ?? false))
                    {
                        await _dialogService.ShowAsync(Alert.Info(
                            response.Content?.Message ?? $"{Email} {AppStrings.Common_Msg_NotAvailable}"));

                        return;
                    }

                    await Task.WhenAny([
                        Toast.Make(response.Content.Message).Show(),
                        _navigation.NavigateToAsync<SignUp_Step1_Page>(ShellNavigationParams.Create(("email", Email!)))
                        ]);
                }
                else
                {
                    await _dialogService.ShowInfoOrThrowAsync(response);
                }
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private async Task SignInAsync()
        {
            using var _ = StartScopedUserInteraction();

            try
            {
                await _navigation.NavigateToRootAsync<SignInPage>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private void SignUpWithGoogle()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private void SignUpWithFacebook()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }
    }
}
