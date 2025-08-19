using Grpc.Core;
using Learnix.Commons.Application.Cache;
using Learnix.Commons.Contracts.Users.Protos;
using MidR.Interfaces;
using Users.Application.Users.UseCases.GetPermissions;

namespace Users.WebApi.gRPC
{
    internal sealed class GetUserPermissionsService(IMediator mediator) : UserPermissionsService.UserPermissionsServiceBase
    {
        public override async Task<GetUserPermissionsResponse> GetUserPermissions(GetUserPermissionsRequest request, ServerCallContext context)
        {
            var result = await mediator.SendAsync(new GetUserPermissionsQuery(request.IdentityId), context.CancellationToken).ConfigureAwait(false);
            if (result.IsFailure)
            {
                throw new RpcException(new Status(StatusCode.NotFound, result.Error!.Description));
            }

            var response = new GetUserPermissionsResponse { UserId = result.Value.UserId.ToString() };

            response.Permissions.Add(result.Value.Permissions);

            return response;
        }
    }
}