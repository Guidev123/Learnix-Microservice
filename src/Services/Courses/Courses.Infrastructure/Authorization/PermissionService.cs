using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Cache;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Contracts.Users.Protos;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.Infrastructure.Extensions;

namespace Courses.Infrastructure.Authorization
{
    public sealed class PermissionService(UserPermissionsService.UserPermissionsServiceClient permissionsServiceClient, ICacheService cacheService) : IPermissionService
    {
        public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var cachedResult = await cacheService.GetAsync<PermissionResponse>(identityId, cancellationToken);
            if (cachedResult is not null)
            {
                return cachedResult;
            }

            var grpcResult = await ExecuteGrpcCall(identityId, cancellationToken);

            if (grpcResult.IsFailure)
            {
                throw new LearnixException(nameof(GetUserPermissionsRequest), grpcResult.Error);
            }

            if (!Guid.TryParse(grpcResult.Value.UserId, out var userId))
            {
                throw new LearnixException(nameof(GetUserPermissionsRequest), Error.Problem(
                    "Users.InvalidUserId",
                    "The user ID retrieved is not a valid GUID"));
            }
            var result = new PermissionResponse(userId, grpcResult.Value.Permissions.ToHashSet());

            await cacheService.SetAsync(result, identityId, cancellationToken: cancellationToken);

            return result;
        }

        private async Task<Result<GetUserPermissionsResponse>> ExecuteGrpcCall(string identityId, CancellationToken cancellationToken)
            => await permissionsServiceClient.GetUserPermissionsAsync(
                    new GetUserPermissionsRequest { IdentityId = identityId },
                    cancellationToken: cancellationToken
                ).ExecuteGrpcCallAsync(nameof(GetUserPermissionsRequest), cancellationToken).ConfigureAwait(false);
    }
}