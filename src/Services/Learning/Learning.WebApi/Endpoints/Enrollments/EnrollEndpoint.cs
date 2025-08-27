using Learning.Application.Enrollments.UseCases.Enroll;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.WebApi.Endpoints;
using Learnix.Commons.WebApi.Extensions;
using MidR.Interfaces;
using System.Security.Claims;

namespace Learning.WebApi.Endpoints.Enrollments
{
    internal sealed class EnrollEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/enrollments", async (EnrollCommand command, ClaimsPrincipal claimsPrincipal, ISender sender) =>
            {
                command.SetStudentId(claimsPrincipal.GetUserId());
                var result = await sender.SendAsync(command);

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Enrollments).RequireAuthorization(PolicyExtensions.EnrollStudent);
        }
    }
}