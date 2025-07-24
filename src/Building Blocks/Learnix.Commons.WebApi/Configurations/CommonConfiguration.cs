using Learnix.Commons.Application.Authorization;
using Learnix.Commons.WebApi.Authorization;
using Learnix.Commons.WebApi.Extensions;
using Learnix.Commons.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Learnix.Commons.WebApi.Configurations
{
    public static class CommonConfiguration
    {
        public static WebApplicationBuilder AddCommonApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.AddSwaggerConfig();

            builder.AddExceptionHandler();

            builder.Host.UseSerilog((context, loggerConfig)
                => loggerConfig.ReadFrom.Configuration(context.Configuration));

            return builder;
        }

        public static WebApplicationBuilder AddSecurity<TPermissionImplementation>(this WebApplicationBuilder builder)
            where TPermissionImplementation : class, IPermissionService
        {
            builder.Services.AddAuthentication().AddJwtBearer();

            builder.Services.AddAuthorization();

            builder.Services.AddHttpContextAccessor();

            builder.Services.ConfigureOptions<JwtBearerConfigureExtensions>();

            builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

            builder.Services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

            builder.Services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            return builder;
        }

        private static WebApplicationBuilder AddExceptionHandler(this WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            return builder;
        }

        public static WebApplication UseCommonPipeline(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseExceptionHandler();

            app.MapOpenApi();
            app.UseSwaggerConfig();

            if (app.Environment.IsDevelopment())
            {
                app.UseSerilogRequestLogging();
                app.LogContextTraceLoggingMiddleware();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        private static IApplicationBuilder LogContextTraceLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogContextTraceLoggingMiddleware>();

            return app;
        }
    }
}