using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure;
using Learnix.Commons.Infrastructure.Http;
using Learnix.Commons.Infrastructure.Outbox.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MidR.MemoryQueue.Interfaces;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Users.Application;
using Users.Application.Abstractions.Identity;
using Users.Domain.Users.Interfaces;
using Users.Infrastructure.Identity;
using Users.Infrastructure.Outbox;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Users.Repositories;

namespace Users.Infrastructure
{
    public static class UsersModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("Database") ?? string.Empty;

            services
                .AddApplication(AssemblyReference.Assembly)
                .AddHandlerDecorators()
                .AddData(configuration)
                .AddKafkaMessageBus(configuration)
                .AddBackgroundJobs()
                .AddTracing()
                .AddDataAccess(dbConnectionString)
                .AddOutboxPattern(configuration)
                .AddHttpClientServices(configuration);

            return services;
        }

        private static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string dbConnectionString)
        {
            services.AddDbContext<UsersDbContext>((scope, options) =>
            {
                options.UseSqlServer(dbConnectionString);
                var outboxInterceptor = scope.GetRequiredService<InsertOutboxMessagesInterceptor>();
                options.AddInterceptors(outboxInterceptor);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork>(scope => scope.GetRequiredService<UsersDbContext>());

            return services;
        }

        private static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyCloakOptions>(configuration.GetSection("Users:Keycloak"));

            services.AddTransient<KeyCloakAuthDelegatingHandler>();

            services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                var keyCloakOptions = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            }).AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>()
            .ConfigurePrimaryHttpMessageHandler(HttpMessageHandlerFactory.CreateSocketsHttpHandler)
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
            .AddResilienceHandler(nameof(ResiliencePipelineExtensions), pipeline => pipeline.ConfigureResilience());

            services.AddTransient<IIdentityProviderService, IdentityProviderService>();

            return services;
        }

        private static IServiceCollection AddTracing(this IServiceCollection services)
        {
            services
            .AddOpenTelemetry()
            .ConfigureResource(c => c.AddService("Learnix.Users.WebApi"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation();

                tracing.AddOtlpExporter();
            });

            return services;
        }

        private static IServiceCollection AddOutboxPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomainEventHandlers();

            services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
        {
            var domainEventHandlers = AssemblyReference.Assembly
                .GetTypes()
                .Where(c => c.IsAssignableTo(typeof(IDomainEventHandler)))
                .ToArray();

            foreach (var domainEventHandler in domainEventHandlers)
            {
                services.TryAddTransient(domainEventHandler);

                var domainEvent = domainEventHandler
                    .GetInterfaces()
                    .Single(c => c.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler = typeof(IdempotentDomainEventHandlerDecorator<>)
                    .MakeGenericType(domainEvent);

                services.Decorate(domainEventHandler, closedIdempotentHandler);
            }

            return services;
        }
    }
}