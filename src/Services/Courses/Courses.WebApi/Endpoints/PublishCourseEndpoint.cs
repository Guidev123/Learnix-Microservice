using Courses.Application.Courses.UseCases.Publish;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;

namespace Courses.WebApi.Endpoints
{
    internal sealed class PublishCourseEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/courses/{courseId:guid}/publish", async (Guid courseId, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new PublishCourseCommand(courseId));

                return result.Match(Results.NoContent, ApiResults.Problem);
            }
            ).WithTags(Tags.Courses)
            .RequireAuthorization(PolicyExtensions.PublishCourse);
        }
    }
}