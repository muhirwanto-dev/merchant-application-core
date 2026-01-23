using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.Domain.Catalogs.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.ViewModels
{
    public partial class OrderItemViewModel(Product _product) : ObservableObject
    {
        public Product Product => _product;

        public double UnitPrice => Product.Price;

        public double UnitNetPrice => UnitPrice - UnitDiscount;

        public double FinalUnitPrice => UnitNetPrice + UnitTax;

        public double TotalPrice => FinalUnitPrice * Quantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private int _quantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UnitNetPrice))]
        [NotifyPropertyChangedFor(nameof(FinalUnitPrice))]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private double _unitDiscount;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinalUnitPrice))]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private double _unitTax;

        [ObservableProperty]
        private string? _notes;
    }
}
