using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.Domain.Inventories.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels
{
    public partial class ProductComponentViewModel(Inventory @inventory) : ObservableRecipient
    {
        [ObservableProperty]
        private long _productId;

        [ObservableProperty]
        private double _quantityPerUnit = double.Min(1.0, @inventory.Stock.Value);

        [ObservableProperty]
        private Inventory _inventory = @inventory;
    }
}
