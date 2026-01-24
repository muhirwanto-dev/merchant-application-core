namespace JualIn.App.Mobile.Core.Infrastructure.Caches.Abstractions
{
    public interface ILocalCache
    {
        public Task<string?> GetAsync(string key);

        public Task SetAsync(string key, string value);
    }
}
