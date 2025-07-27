using Learning.Application;
using Learning.Infrastructure.Inbox;
using Learning.Infrastructure.Outbox;
using Learnix.Commons.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidR.MemoryQueue.Interfaces;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Learning.Infrastructure
{
    public static class LearningModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("Database") ?? string.Empty;

            services
                .AddApplication(AssemblyReference.Assembly)
                .AddHandlerDecorators()
                .AddOutboxPattern(configuration)
                .AddInboxPattern(configuration)
                .AddData(configuration)
                .AddKafkaMessageBus(configuration)
                .AddBackgroundJobs()
                .AddTracing()
                .AddDataAccess(dbConnectionString)
                .AddTracing()
                .AddIntegrationEvents();

            return services;
        }

        private static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string dbConnectionString)
        {
            return services;
        }

        private static IServiceCollection AddOutboxPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandlerDecorator<>));

            services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddInboxPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentIntegrationEventHandlerDecorator<>));

            services.Configure<InboxOptions>(configuration.GetSection(nameof(InboxOptions)));
            services.ConfigureOptions<ConfigureProcessInboxJob>();

            return services;
        }

        private static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddHostedService<IntegrationEventsConsumer>();

            return services;
        }

        private static IServiceCollection AddTracing(this IServiceCollection services)
        {
            services
            .AddOpenTelemetry()
            .ConfigureResource(c => c.AddService("Learnix.Learning.WebApi"))
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
    }
}