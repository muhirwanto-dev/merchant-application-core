namespace JualIn.App.Mobile.Core.Modules.Catalogs.Abstractions
{
    public interface IProductService
    {
        Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
