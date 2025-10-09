using Courses.Application.Features.Create;
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
            app.MapPost("api/v1/courses", async (CreateCourseCommand command, ISender sender) =>
            {
                var result = await sender.SendAsync(command).ConfigureAwait(false);

                return result.Match(successResult => Results.Created($"api/v1/courses/{successResult.CourseId}",
                                    successResult), ApiResults.Problem);
            }
            ).WithTags(Tags.Courses)
            .RequireAuthorization(PolicyExtensions.CreateCourse);
        }
    }
}