using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;
using Users.Application.Features.GetById;

namespace Users.WebApi.Endpoints
{
    internal sealed class GetUserByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/me", async (
                ClaimsPrincipal claimsPrincipal,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.SendAsync(new GetUserByIdQuery(claimsPrincipal.GetUserId()), cancellationToken);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Users).RequireAuthorization(PolicyExtensions.GetUser);
        }
    }
}