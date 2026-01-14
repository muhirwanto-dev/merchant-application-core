using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Modules.Catalogs.ViewModels;
using JualIn.App.Mobile.Presentation.Modules.Inventories.ViewModels;
using JualIn.Domain.Inventories.Entities;
using Riok.Mapperly.Abstractions;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Mapping
{
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public partial class Mapper : IMapper
    {
        public partial TResult Map<TSource, TResult>(TSource source);

        public void MapTo<TSource, TResult>(TSource source, TResult result)
        {
            switch (source)
            {
                case InventoryViewModel x when typeof(TResult).IsAssignableFrom(typeof(InventoryFormViewModel)):
                    MapToForm(x, (InventoryFormViewModel)(object)result!);
                    break;
                case Inventory x when typeof(TResult).IsAssignableFrom(typeof(InventoryFormViewModel)):
                    MapToForm(x, (InventoryFormViewModel)(object)result!);
                    break;
                case ProductViewModel x when typeof(TResult).IsAssignableFrom(typeof(ProductFormViewModel)):
                    MapToForm(x, (ProductFormViewModel)(object)result!);
                    break;
                case Product x when typeof(TResult).IsAssignableFrom(typeof(ProductFormViewModel)):
                    MapToForm(x, (ProductFormViewModel)(object)result!);
                    break;
                case null:
                    ThrowHelper.ThrowArgumentNullException(nameof(source));
                    break;
                default:
                    ThrowHelper.ThrowArgumentException($"Cannot map {source.GetType()} to {typeof(void)} as there is no known type mapping", nameof(source));
                    break;
            }
            ;
        }
    }
}
