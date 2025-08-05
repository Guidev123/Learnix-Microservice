using Learning.Application;
using Learning.Domain.Students.Interfaces;
using Learning.Infrastructure.Inbox;
using Learning.Infrastructure.Outbox;
using Learning.Infrastructure.Persistence;
using Learning.Infrastructure.Students.Repositories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure;
using Learnix.Commons.Infrastructure.Inbox.Models;
using Learnix.Commons.Infrastructure.Outbox.Interceptors;
using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                .AddData(configuration)
                .AddOutboxPattern(configuration)
                .AddInboxPattern(configuration)
                .AddKafkaMessageBus(configuration)
                .AddBackgroundJobs()
                .AddTracing()
                .AddDataAccess(dbConnectionString)
                .AddTracing();

            return services;
        }

        private static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string dbConnectionString)
        {
            services.AddDbContext<LearningDbContext>((scope, options) =>
            {
                options.UseSqlServer(dbConnectionString);
                var outboxInterceptor = scope.GetRequiredService<InsertOutboxMessagesInterceptor>();
                options.AddInterceptors(outboxInterceptor);
            });

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUnitOfWork>(scope => scope.GetRequiredService<LearningDbContext>());

            return services;
        }

        private static IServiceCollection AddOutboxPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomainEventHandlers();

            services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));
            services.ConfigureOptions<ConfigureProcessOutboxJob>();

            return services;
        }

        private static IServiceCollection AddInboxPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<IntegrationEventsConsumer>();

            services.AddIntegrationEventHandlers();

            services.Configure<InboxOptions>(configuration.GetSection(nameof(InboxOptions)));
            services.ConfigureOptions<ConfigureProcessInboxJob>();

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

        private static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var integrationEventHandlers = typeof(LearningModule).Assembly.GetTypes()
                 .Where(c => c.IsAssignableTo(typeof(IIntegrationEventHandler)) && !c.IsAbstract && !c.IsInterface)
                 .Where(c => !c.Name.Contains(nameof(IdempotentIntegrationEventHandlerDecorator<IntegrationEvent>)))
                 .Where(c => !c.IsGenericTypeDefinition)
                 .Where(c => c.IsClass && !c.IsAbstract)
                 .ToArray();

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                services.TryAddTransient(integrationEventHandler);

                var integrationEvent = integrationEventHandler
                    .GetInterfaces()
                    .Single(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandler = typeof(IdempotentIntegrationEventHandlerDecorator<>)
                    .MakeGenericType(integrationEvent);

                services.Decorate(integrationEventHandler, closedIdempotentHandler);
            }

            return services;
        }
    }
}