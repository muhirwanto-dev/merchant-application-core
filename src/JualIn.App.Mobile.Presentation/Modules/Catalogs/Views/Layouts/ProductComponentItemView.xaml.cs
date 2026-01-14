using JualIn.App.Mobile.Presentation.Core.Extensions;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Views.Layouts;

public partial class ProductComponentItemView : ContentView
{
    public ProductComponentItemView()
    {
        InitializeComponent();
    }

    private void QuantityPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {
        var parent = this.GetParentPage<ProductFormPage>();
        if (parent?.BindingContext is ProductFormViewModel vm)
        {
            vm.QuantityPerUnitChangedCommand.Execute(null);
        }
    }
}