namespace JualIn.App.Mobile.Core.Modules.Inventories.Abstractions
{
    public interface IInventoryService
    {
        Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
