using JualIn.App.Mobile.Presentation.Modules.Auth.ViewModels;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.Views;

[ViewModelOwner<SignInViewModel>(IsDefaultConstructor = true)]
public partial class SignInPage : ContentPage;