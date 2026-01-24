namespace JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions
{
    public interface ISearchable<T>
    {
        Task<List<T>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
