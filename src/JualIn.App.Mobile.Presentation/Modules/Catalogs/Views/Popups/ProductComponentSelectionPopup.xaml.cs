using CommunityToolkit.Maui.Views;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels.Popups;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Views.Popups;

[ViewModelOwner<ProductComponentSelectionPopupViewModel>(IsDefaultConstructor = true)]
public partial class ProductComponentSelectionPopup : Popup;