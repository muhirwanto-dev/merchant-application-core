namespace JualIn.App.Mobile.Presentation.Modules.Inventories.Abstractions
{
    public interface IInventoryService
    {
        Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
