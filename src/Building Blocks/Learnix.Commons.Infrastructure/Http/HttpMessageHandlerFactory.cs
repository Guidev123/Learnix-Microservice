namespace Learnix.Commons.Infrastructure.Http
{
    public static class HttpMessageHandlerFactory
    {
        private static readonly TimeSpan _httpRestHandlerPooledConnectionLifetime = TimeSpan.FromSeconds(5);

        public static SocketsHttpHandler CreateSocketsHttpRestHandler()
            => new()
            { PooledConnectionLifetime = _httpRestHandlerPooledConnectionLifetime };
    }
}