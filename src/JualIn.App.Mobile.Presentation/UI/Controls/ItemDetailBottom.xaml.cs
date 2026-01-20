using Plugin.Maui.BottomSheet;
using SingleScope.Mvvm.Attributes;

namespace JualIn.App.Mobile.Presentation.UI.Controls;

[ViewModelOwner<ItemDetailBottomViewModel>(IsDefaultConstructor = true)]
public partial class ItemDetailBottom : BottomSheet;