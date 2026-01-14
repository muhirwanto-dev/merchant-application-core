using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.Domain.Inventories.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels
{
    public partial class InventoryViewModel(Inventory @entity) : ObservableObject
    {
        [ObservableProperty]
        private Inventory _entity = @entity;

        public bool IsLowStock => Entity.Stock <= Entity.StockThreshold;

        public bool IsExpired => DateTime.UtcNow >= Entity.ExpirationDate;
    }
}
