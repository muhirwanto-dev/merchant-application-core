using CommunityToolkit.Mvvm.ComponentModel;
using JualIn.Domain.Catalogs.Entities;

namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels
{
    public partial class ProductViewModel(Product @entity) : ObservableRecipient
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasAnyComponent))]
        private Product _entity = @entity;

        public bool HasAnyComponent => Entity.Components.Count > 0;
    }
}
