using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.Domain.Catalogs.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels;

public partial class SaleProductViewModel(Product @entity) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OnCartQuantity))]
    private Product _entity = @entity;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OnCartTotalPrice))]
    [NotifyPropertyChangedFor(nameof(IsSelected))]
    private int _onCartQuantity;

    public double OnCartTotalPrice => Entity.Price * OnCartQuantity;

    public bool IsSelected => OnCartQuantity > 0;
}
