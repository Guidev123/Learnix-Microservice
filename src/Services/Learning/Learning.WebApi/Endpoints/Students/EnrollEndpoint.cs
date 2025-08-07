using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;

namespace Learning.WebApi.Endpoints.Students
{
    internal sealed class EnrollEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/enrollments", async () =>
            {
            }).RequireAuthorization(PolicyExtensions.EnrollStudent);
        }
    }
}