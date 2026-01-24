using Ardalis.SmartEnum;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Models
{
    public class InventoryFilter(string name, string value)
        : SmartEnum<InventoryFilter, string>(name, value);
}
