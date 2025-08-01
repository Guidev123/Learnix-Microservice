﻿using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.MemoryQueue.Interfaces;
using Users.Application.Users.UseCases.Register;

namespace Users.WebApi.Endpoints
{
    internal sealed class RegisterUserEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/users", async (RegisterUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.DispatchAsync(command);

                return result.Match(response => Results.Created("api/v1/users/me", response), ApiResults.Problem);
            }).WithTags(Tags.Users);
        }
    }
}