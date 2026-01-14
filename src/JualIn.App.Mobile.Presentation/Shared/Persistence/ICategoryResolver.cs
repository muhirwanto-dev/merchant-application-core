namespace JualIn.App.Mobile.Presentation.Shared.Persistence
{
    public interface ICategoryResolver<T>
    {
        Task<string[]> GetCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
