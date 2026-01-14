using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Views;

[ViewModelOwner<InventoryManagementViewModel>(IsDefaultConstructor = true)]
public partial class InventoryManagementPage : ContentPage;