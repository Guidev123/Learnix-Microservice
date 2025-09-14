using Courses.Infrastructure;
using Courses.Infrastructure.Authorization;
using Learnix.Commons.WebApi.Configurations;
using Learnix.Commons.WebApi.Extensions;
using System.Reflection;

namespace Courses.WebApi.Configurations
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

        public static WebApplication UsePipeline(this WebApplication app)
        {
            app.UseCommonPipeline();
            app.MapEndpoints();

            return app;
        }
    }
}