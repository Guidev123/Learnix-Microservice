using Learning.Application.Features.GetCourseContent;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;

namespace Learning.WebApi.Endpoints
{
    internal sealed class GetCourseContentEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/learning/courses/{courseId:guid}", async (ClaimsPrincipal claimsPrincipal, Guid courseId, ISender sender) =>
            {
                var result = await sender.SendAsync(new GetCourseContentQuery(courseId, claimsPrincipal.GetUserId()));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetCourseContent)
            .WithTags(Tags.Courses);
        }
    }
}