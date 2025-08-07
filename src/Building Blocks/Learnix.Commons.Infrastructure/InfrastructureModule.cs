using FluentValidation;
using Learnix.Commons.Application.Cache;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Decorators;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Infrastructure.Cache;
using Learnix.Commons.Infrastructure.Clock;
using Learnix.Commons.Infrastructure.Factories;
using Learnix.Commons.Infrastructure.MessageBus;
using Learnix.Commons.Infrastructure.Outbox.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MidR.DependencyInjection;
using MidR.Interfaces;
using Quartz;
using StackExchange.Redis;
using System.Reflection;

namespace Learnix.Commons.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddMidR(applicationAssembly);

            services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);

            services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }

        public static IServiceCollection AddHandlerDecorators(this IServiceCollection services)
        {
            services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
            services.Decorate(typeof(IRequestHandler<,>), typeof(RequestLoggingDecorator<,>));

            return services;
        }

        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConnectionString = configuration.GetConnectionString("Database") ?? string.Empty;

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>(sp =>
            {
                return new SqlConnectionFactory(databaseConnectionString);
            });

            services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

            return services;
        }

        public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache") ?? string.Empty);
                services.TryAddSingleton(connectionMultiplexer);

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
                });

                services.TryAddSingleton<ICacheService, CacheService>();
            }
            catch
            {
                services.TryAddSingleton<ICacheService, CacheService>();
                services.AddDistributedMemoryCache();
            }

            return services;
        }

        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddQuartz(c =>
            {
                var schedulerId = Guid.NewGuid();
                c.SchedulerId = $"default-id-{schedulerId}";
                c.SchedulerName = $"dafault-name-{schedulerId}";
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            return services;
        }

        public static IServiceCollection AddKafkaMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageBusOptions>(configuration.GetSection(nameof(MessageBusOptions)));

            services.TryAddSingleton<IMessageBus, MessageBus.MessageBus>();

            return services;
        }
    }
}