namespace JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions
{
    public interface ICategoryResolver<T>
    {
        Task<string[]> GetCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
