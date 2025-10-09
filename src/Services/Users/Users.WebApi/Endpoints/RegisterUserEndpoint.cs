using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using Users.Application.Features.Register;

namespace Users.WebApi.Endpoints
{
    internal sealed class RegisterUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/users", async (RegisterUserCommand command, ISender sender) =>
            {
                var result = await sender.SendAsync(command);

                return result.Match(response => Results.Created("api/v1/users/me", response), ApiResults.Problem);
            }).WithTags(Tags.Users);
        }
    }
}