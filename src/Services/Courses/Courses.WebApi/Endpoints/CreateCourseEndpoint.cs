using Courses.Application.Courses.UseCases.Create;
using Courses.Application.Courses.UseCases.GetContent;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;

namespace Courses.WebApi.Endpoints
{
    internal sealed class CreateCourseEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/courses", async (CreateCourseCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command).ConfigureAwait(false);

                return result.Match(successResult => Results.Created($"api/v1/courses/{successResult.CourseId}",
                                    successResult), ApiResults.Problem);
            }
            ).WithTags(Tags.Courses)
            /*.RequireAuthorization(PolicyExtensions.CreateCourse)*/;
        }
    }

    internal sealed class GetCourseContentEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/courses/{courseId:guid}/content", async (Guid courseId, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetCourseContentQuery(courseId)).ConfigureAwait(false);

                return result.Match(successResult => Results.Ok(successResult), ApiResults.Problem);
            }).WithTags(Tags.Courses);
        }
    }
}