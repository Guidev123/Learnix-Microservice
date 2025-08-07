namespace Learnix.Commons.Application.Cache
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : class;

        Task SetAsync<T>(T data, string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
            where T : class;

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}