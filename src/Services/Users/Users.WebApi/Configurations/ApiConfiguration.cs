using Learnix.Commons.WebApi.Configurations;
using Learnix.Commons.WebApi.Extensions;
using System.Reflection;
using Users.Infrastructure;
using Users.Infrastructure.Authorization;
using Users.WebApi.gRPC;

namespace Users.WebApi.Configurations
{
    public static class ApiConfiguration
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder
                .AddCommonApiConfiguration()
                .AddSecurity<PermissionService>()
                .Services
                .AddInfrastructureModule(builder.Configuration)
                .AddEndpoints(Assembly.GetExecutingAssembly());

            return builder;
        }

        public static WebApplication UsePipeline(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseCommonPipeline(builder);
            app.MapEndpoints();
            app.MapGrpcServices();

            return app;
        }

        private static WebApplication MapGrpcServices(this WebApplication app)
        {
            app.MapGrpcService<GetUserPermissionsService>();

            return app;
        }
    }
}