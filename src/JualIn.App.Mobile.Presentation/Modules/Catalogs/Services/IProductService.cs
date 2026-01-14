namespace JualIn.App.Mobile.Presentation.Modules.Catalogs.Services
{
    public interface IProductService
    {
        Task UpdateStockAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
