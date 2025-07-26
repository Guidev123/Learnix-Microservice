using Learnix.Commons.Application;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure;
using Learnix.Commons.Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Users.Application;
using Users.Application.Abstractions.Identity;
using Users.Domain.Interfaces;
using Users.Infrastructure.Identity;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;

namespace Users.Infrastructure
{
    public static class UsersModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("Database") ?? string.Empty;

            services
                .AddCommonInfrastructure(AssemblyReference.Assembly, configuration)
                .AddTracing()
                .AddDataAccess(dbConnectionString)
                .AddHttpClientServices(configuration);

            return services;
        }

        private static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string dbConnectionString)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
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
    }
}