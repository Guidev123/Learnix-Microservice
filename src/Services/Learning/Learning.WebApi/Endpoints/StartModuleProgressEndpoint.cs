using Learning.Application.Features.StartModuleProgress;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;

namespace Learning.WebApi.Endpoints
{
    internal sealed class StartModuleProgressEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/learning/progress/modules/start", async (
                StartModuleProgressCommand command,
                ISender sender,
                ClaimsPrincipal claimsPrincipal,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.SendAsync(command.SetStudentId(claimsPrincipal.GetUserId()), cancellationToken);

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Progress).RequireAuthorization(PolicyExtensions.ManageLessonProgress);
        }
    }
}