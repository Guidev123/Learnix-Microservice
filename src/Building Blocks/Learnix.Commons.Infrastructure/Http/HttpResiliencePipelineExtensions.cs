using Grpc.Core;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Learnix.Commons.Infrastructure.Http
{
    public static class HttpResiliencePipelineExtensions
    {
        private static readonly double _failureRatio = 0.9;
        private static readonly int _maxRetriesAttempts = 3;
        private static readonly int _minimumThroughput = 5;
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan _defaultRetryDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan _breakDuration = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan _samplingDuration = TimeSpan.FromSeconds(10);

        public static void ConfigureHttpRestResilience(this ResiliencePipelineBuilder<HttpResponseMessage> pipeline)
        {
            pipeline.AddTimeout(_timeout);

            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = _maxRetriesAttempts,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = _defaultRetryDelay
            });

            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = _samplingDuration,
                FailureRatio = _failureRatio,
                MinimumThroughput = _minimumThroughput,
                BreakDuration = _breakDuration
            });
        }

        public static void ConfigureGrpcResilience<T>(this ResiliencePipelineBuilder<T> pipeline)
        {
            pipeline.AddTimeout(_timeout);

            pipeline.AddRetry(new RetryStrategyOptions<T>
            {
                MaxRetryAttempts = _maxRetriesAttempts,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = _defaultRetryDelay,
                ShouldHandle = new PredicateBuilder<T>()
                    .HandleResult(static result => ShouldRetryGrpcResult(result))
                    .Handle<RpcException>(static ex => ShouldRetryGrpcException(ex))
            });

            pipeline.AddCircuitBreaker(new CircuitBreakerStrategyOptions<T>
            {
                SamplingDuration = _samplingDuration,
                FailureRatio = _failureRatio,
                MinimumThroughput = _minimumThroughput,
                BreakDuration = _breakDuration,
                ShouldHandle = new PredicateBuilder<T>()
                    .HandleResult(static result => ShouldHandleForCircuitBreaker(result))
                    .Handle<RpcException>(static ex => ShouldHandleGrpcExceptionForCircuitBreaker(ex))
            });
        }

        private static bool ShouldRetryGrpcException(RpcException exception)
        {
            return exception.StatusCode switch
            {
                StatusCode.Unavailable => true,
                StatusCode.DeadlineExceeded => true,
                StatusCode.ResourceExhausted => true,
                StatusCode.Aborted => true,
                StatusCode.Internal => true,
                StatusCode.InvalidArgument => false,
                StatusCode.NotFound => false,
                StatusCode.AlreadyExists => false,
                StatusCode.PermissionDenied => false,
                StatusCode.Unauthenticated => false,
                StatusCode.FailedPrecondition => false,
                StatusCode.OutOfRange => false,
                StatusCode.Unimplemented => false,
                _ => false
            };
        }

        private static bool ShouldHandleForCircuitBreaker<T>(T result)
        {
            return false;
        }

        private static bool ShouldRetryGrpcResult<T>(T result)
        {
            return false;
        }

        private static bool ShouldHandleGrpcExceptionForCircuitBreaker(RpcException exception)
        {
            return exception.StatusCode switch
            {
                StatusCode.Unavailable => true,
                StatusCode.DeadlineExceeded => true,
                StatusCode.ResourceExhausted => true,
                StatusCode.Internal => true,
                StatusCode.InvalidArgument => false,
                StatusCode.Unauthenticated => false,
                StatusCode.PermissionDenied => false,
                StatusCode.NotFound => false,
                _ => true
            };
        }
    }
}