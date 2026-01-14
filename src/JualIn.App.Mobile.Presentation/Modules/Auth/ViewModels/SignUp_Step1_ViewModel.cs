using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope;
using JualIn.App.Mobile.Presentation.Core.ViewModels;
using JualIn.App.Mobile.Presentation.Infrastructure.Api;
using JualIn.App.Mobile.Presentation.Modules.Auth.Views;
using JualIn.App.Mobile.Presentation.Resources.Strings;
using JualIn.Contracts.Dtos.Auth.EmailSignUp;
using JualIn.Contracts.Enums;
using SingleScope.Maui.Dialogs;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.ViewModels
{
    [QueryProperty(nameof(Email), "email")]
    public partial class SignUp_Step1_ViewModel : BaseViewModel
    {
        private readonly IBackendApi _api;

        public IEnumerable<string> BusinessCategorySuggestions { get; } = [.. Domain.Account.ValueObjects.BusinessCategory.List.Select(x => x.Value)];

        [ObservableProperty] private string? _email;
        [ObservableProperty] private string? _password;
        [ObservableProperty] private string? _confirmPassword;
        [ObservableProperty] private string? _firstName;
        [ObservableProperty] private string? _lastName;
        [ObservableProperty] private string? _businessName;
        [ObservableProperty] private string? _businessCategory;

        public SignUp_Step1_ViewModel(
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
        private async Task RegisterAsync()
        {
            try
            {
                if (Password != ConfirmPassword)
                {
                    await _dialogService.ShowAsync(Alert.Info(AppStrings.SignUpPage_Lbl_PasswordNotMatch));

                    return;
                }

                using var _ = _loadingService.Show();
                var response = await _api.SignUpAsync(new EmailSignUpRequestDto(
                    Email!,
                    Password!,
                    FirstName!,
                    LastName,
                    BusinessName,
                    Enum.TryParse<BusinessCategory>(BusinessCategory, out var category) ? category : null
                    ));

                if (response.IsSuccessful && response.Content is EmailSignUpResponseDto dto)
                {
                    await Toast.Make(AppStrings.SignUpPage_Msg_AccountRegistered).Show();
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
