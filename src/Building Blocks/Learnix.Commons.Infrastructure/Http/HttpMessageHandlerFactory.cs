namespace Learnix.Commons.Infrastructure.Http
{
    public static class HttpMessageHandlerFactory
    {
        private static readonly TimeSpan _httpRestHandlerPooledConnectionLifetime = TimeSpan.FromSeconds(5);

        private static readonly TimeSpan _grpcPooledConnectionLifeTime = TimeSpan.FromMinutes(5);

        public static SocketsHttpHandler CreateSocketsHttpRestHandler()
            => new()
            { PooledConnectionLifetime = _httpRestHandlerPooledConnectionLifetime };

        public static SocketsHttpHandler CreateGrpcSocketsHttpHandler()
            => new()
            {
                PooledConnectionLifetime = _grpcPooledConnectionLifeTime,
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests
            };
    }
}