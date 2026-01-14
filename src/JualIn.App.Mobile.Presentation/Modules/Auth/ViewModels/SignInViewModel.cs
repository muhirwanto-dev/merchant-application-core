using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Api;
using JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Auth.Views;
using JualIn.App.Mobile.Presentation.Modules.Dashboard.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.Contracts.Dtos.Auth.EmailSignIn;
using SingleScope.Maui.Dialogs;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.ViewModels
{
    public partial class SignInViewModel : BaseViewModel
    {
        private readonly IBackendApi _api;
        private readonly IAuthService _authService;

        [ObservableProperty] private string? _email = "dummymail.general@gmail.com";
        [ObservableProperty] private string? _password = "LLAdortoh123!";

        public SignInViewModel(
            IBackendApi belibuApi,
            IAuthService authService)
        {
            _api = belibuApi;
            _authService = authService;

            RegisterInteractionCommand(nameof(IsUserInteraction), SignInCommand);
            RegisterInteractionCommand(nameof(IsUserInteraction), SignUpCommand);
        }

        [RelayCommand]
        private async Task AppearingAsync()
        {
            try
            {
                await SignInAsync();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private async Task SignInAsync()
        {
            try
            {
                using var _1 = StartScopedUserInteraction();
                using var _2 = _loadingService.Show();

                var response = await _api.SignInAsync(new EmailSignInRequestDto(Email!, Password!, RememberMe: false));
                if (response.IsSuccessful && response.Content is EmailSignInResponseDto dto)
                {
                    if (!dto.IsEmailConfirmed)
                    {
                        await _dialogService.ShowAsync(Alert.Info(AppStrings.SignInPage_Msg_EmailNotConfirmed));
                        return;
                    }

                    await _authService.SaveSignInDataAsync(dto.UserIdentifier, dto.AccessToken, dto.RefreshToken, dto.Expiration);
                    await Task.WhenAll([
                        _authService.FetchUserDataAsync().AsTask(),
                        _navigation.NavigateToRootAsync<DashboardPage>()
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
        private async Task SignUpAsync()
        {
            using var _ = StartScopedUserInteraction();

            try
            {
                await _navigation.NavigateToAsync<SignUp_Step0_Page>();
            }
            catch (Exception ex)
            {
                _reporting.ReportProblems(ex);
            }
        }

        [RelayCommand(CanExecute = nameof(CanUserInteraction))]
        private void ForgotPassword()
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
        private void SignInWithGoogle()
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
        private void SignInWithFacebook()
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
