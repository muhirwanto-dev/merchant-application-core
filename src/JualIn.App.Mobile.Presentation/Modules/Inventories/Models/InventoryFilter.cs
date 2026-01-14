using Ardalis.SmartEnum;
using JualIn.App.Mobile.Presentation.Resources.Strings;

namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Models
{
    public class InventoryFilter(string name, string value)
        : SmartEnum<InventoryFilter, string>(name, value)
    {
        public static readonly InventoryFilter AllItems = new(nameof(AllItems), AppStrings.InventoryDetail_Filter_AllItems);
        public static readonly InventoryFilter LowStock = new(nameof(LowStock), AppStrings.InventoryDetail_Filter_LowStock);
        public static readonly InventoryFilter Expired = new(nameof(Expired), AppStrings.InventoryDetail_Filter_Expired);
    }
}
