using JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Views;

[ViewModelOwner<OrderHistoryViewModel>(IsDefaultConstructor = true)]
public partial class OrderHistoryPage : ContentPage;