using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Contracts.Users.Protos;
using Learnix.Commons.Domain.Results;

namespace Learning.Infrastructure.Authorization
{
    public sealed class PermissionService(UserPermissionsService.UserPermissionsServiceClient permissionsServiceClient) : IPermissionService
    {
        public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var grpcResponse = await permissionsServiceClient.GetUserPermissionsAsync(new GetUserPermissionsRequest
            {
                IdentityId = identityId
            }, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (grpcResponse is null)
            {
                throw new LearnixException(nameof(GetUserPermissionsRequest), Error.Problem(
                    "Users.FailedToRetrieveUserPermissions",
                    "Failed to retrieve user permissions"));
            }

            if (!Guid.TryParse(grpcResponse.UserId, out var userId))
            {
                throw new LearnixException(nameof(GetUserPermissionsRequest), Error.Problem(
                    "Users.InvalidUserId",
                    "The user ID retrieved is not a valid GUID"));
            }

            return Result.Success(new PermissionResponse(userId, grpcResponse.Permissions.ToHashSet()));
        }
    }
}