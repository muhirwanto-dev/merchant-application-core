namespace JualIn.App.Mobile.Presentation.Shared.Persistence
{
    public interface ISearchable<T>
    {
        Task<List<T>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
