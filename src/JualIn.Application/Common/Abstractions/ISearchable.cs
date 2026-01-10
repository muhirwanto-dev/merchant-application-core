namespace JualIn.Application.Common.Abstractions
{
    public interface ISearchable<T>
    {
        Task<List<T>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
