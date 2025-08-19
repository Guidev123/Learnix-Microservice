using Courses.Application.Courses.UseCases.AttachLessonsToModule;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;

namespace Courses.WebApi.Endpoints
{
    internal sealed class AttachLessonsToModuleEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/courses/{courseId:guid}/modules/{moduleId:guid}/lessons", async (
                Guid courseId,
                Guid moduleId,
                AttachLessonsToModuleCommand command,
                IMediator mediator) =>
            {
                var result = await mediator.SendAsync(
                    command
                        .SetCourseId(courseId)
                        .SetModuleId(moduleId)
                    ).ConfigureAwait(false);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }
            ).WithTags(Tags.Courses)
            .RequireAuthorization(PolicyExtensions.CreateCourse);
        }
    }
}