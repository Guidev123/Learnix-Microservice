using Learning.Application.Features.UpdateLessonProgress;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;

namespace Learning.WebApi.Endpoints
{
    internal sealed class CompleteLessonProgressEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/learning/progress/lessons/complete", async (
                UpdateLessonProgressCommand command,
                ISender sender,
                ClaimsPrincipal claimsPrincipal,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.SendAsync(command.SetStudentId(claimsPrincipal.GetUserId()), cancellationToken);

                return result.Match(Results.NoContent, ApiResults.Problem);
            }).WithTags(Tags.Progress).RequireAuthorization(PolicyExtensions.ManageLessonProgress);
        }
    }
}