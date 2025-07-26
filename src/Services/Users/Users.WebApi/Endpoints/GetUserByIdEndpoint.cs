using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.MemoryQueue.Interfaces;
using System.Security.Claims;
using Users.Application.Users.UseCases.GetById;

namespace Users.WebApi.Endpoints
{
    internal sealed class GetUserByIdEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/users/me", async (ClaimsPrincipal claimsPrincipal, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(new GetUserByIdQuery(claimsPrincipal.GetUserId()));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).RequireAuthorization(PolicyExtensions.GetUser);
        }
    }
}