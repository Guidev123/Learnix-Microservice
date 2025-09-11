using Learning.Application.Enrollments.UseCases.GetCourseContent;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;

namespace Learning.WebApi.Endpoints.Enrollments
{
    internal sealed class GetCourseContentEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/learning/courses/{courseId:guid}", async (Guid courseId, ISender sender) =>
            {
                var result = await sender.SendAsync(new GetCourseContentQuery(courseId));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetCourseContent)
            .WithTags(Tags.Courses);
        }
    }
}