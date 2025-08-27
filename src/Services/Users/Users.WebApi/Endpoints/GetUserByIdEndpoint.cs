using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;
using Users.Application.Users.UseCases.GetById;

namespace Users.WebApi.Endpoints
{
    internal sealed class GetUserByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/me", async (ClaimsPrincipal claimsPrincipal, ISender sender) =>
            {
                var result = await sender.SendAsync(new GetUserByIdQuery(claimsPrincipal.GetUserId()));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Users).RequireAuthorization(PolicyExtensions.GetUser);
        }
    }
}