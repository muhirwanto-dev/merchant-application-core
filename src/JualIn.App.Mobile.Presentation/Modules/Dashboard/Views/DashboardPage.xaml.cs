using JualIn.App.Mobile.Presentation.Modules.Dashboard.ViewModels;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Dashboard.Views;

[ViewModelOwner<DashboardViewModel>(IsDefaultConstructor = true)]
public partial class DashboardPage : ContentPage;