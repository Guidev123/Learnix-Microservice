using Learnix.Commons.Application.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System.Buffers;
using System.Text.Json;

namespace Learnix.Commons.Infrastructure.Cache
{
    internal sealed class CacheService(IDistributedCache cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            byte[]? bytes = await cache.GetAsync(key, cancellationToken).ConfigureAwait(false);

            return bytes is null ? default : Deserialize<T>(bytes);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default) => cache.RemoveAsync(key, cancellationToken);

        public async Task SetAsync<T>(T value, string key, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
        {
            byte[] bytes = Serialize(value);

            await cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken).ConfigureAwait(false);
        }

        private static T Deserialize<T>(byte[] bytes) => JsonSerializer.Deserialize<T>(bytes)!;

        private static byte[] Serialize<T>(T value)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(buffer);
            JsonSerializer.Serialize(writer, value);
            return buffer.WrittenSpan.ToArray();
        }
    }
}