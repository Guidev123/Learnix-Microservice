using Courses.Application.Courses.UseCases.GetContent;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;

namespace Courses.WebApi.Endpoints
{
    internal sealed class GetCourseContentEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/courses/{courseId:guid}/content", async (Guid courseId, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCourseContentQuery(courseId)).ConfigureAwait(false);

                return result.Match(successResult => Results.Ok(successResult), ApiResults.Problem);
            }
            ).WithTags(Tags.Courses)
            .RequireAuthorization(PolicyExtensions.GetCourseContent);
        }
    }
}