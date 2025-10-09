using Grpc.Core;
using Learnix.Commons.Contracts.Users.Protos;
using MidR.Interfaces;
using Users.Application.Features.GetPermissions;

namespace Users.WebApi.gRPC
{
    internal sealed class GetUserPermissionsService(ISender sender) : UserPermissionsService.UserPermissionsServiceBase
    {
        public override async Task<GetUserPermissionsResponse> GetUserPermissions(GetUserPermissionsRequest request, ServerCallContext context)
        {
            var result = await sender.SendAsync(new GetUserPermissionsQuery(request.IdentityId), context.CancellationToken).ConfigureAwait(false);
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