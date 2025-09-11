using Courses.Application;
using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.Interfaces;
using Courses.Infrastructure.Courses.Repositories;
using Courses.Infrastructure.Inbox;
using Courses.Infrastructure.Outbox;
using Courses.Infrastructure.Persistence;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users.Protos;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure;
using Learnix.Commons.Infrastructure.Http;
using Learnix.Commons.Infrastructure.Inbox.Models;
using Learnix.Commons.Infrastructure.Outbox.Interceptors;
using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Courses.Infrastructure
{
    public static class CourseModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("Database") ?? string.Empty;

            services
                .AddApplication(AssemblyReference.Assembly)
                .AddGrpcServices(configuration)
                .AddData(configuration)
                .AddCacheService(configuration)
                .AddBackgroundJobs()
                .AddDataAccess(dbConnectionString)
                .AddKafkaMessageBus(configuration)
                .AddInboxPattern(configuration)
                .AddOutboxPattern(configuration)
                .AddTracing();

            return services;
        }

        private static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string dbConnectionString)
        {
            services.AddDbContext<CourseDbContext>((scope, options) =>
            {
                options.UseSqlServer(dbConnectionString);
                var outboxInterceptor = scope.GetRequiredService<InsertOutboxMessagesInterceptor>();
                options.AddInterceptors(outboxInterceptor);
            });

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUnitOfWork>(scope => scope.GetRequiredService<CourseDbContext>());

            return services;
        }

        private static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpcClient<UserPermissionsService.UserPermissionsServiceClient>(options =>
            {
                options.Address = new Uri(configuration["ExternalServices:UsersApi"]!);
            }).AddResilienceHandler(nameof(HttpResiliencePipelineExtensions), pipeline => pipeline.ConfigureGrpcResilience());

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
            .ConfigureResource(c => c.AddService("Learnix.Courses.WebApi"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources");

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
            var integrationEventHandlers = typeof(Course).Assembly.GetTypes()
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