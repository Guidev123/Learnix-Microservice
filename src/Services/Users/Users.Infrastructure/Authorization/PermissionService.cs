using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Domain.Results;
using MidR.Interfaces;
using Users.Application.Users.UseCases.GetPermissions;

namespace Users.Infrastructure.Authorization
{
    public sealed class PermissionService(ISender sender) : IPermissionService
    {
        public async Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            var result = await sender.SendAsync(new GetUserPermissionsQuery(identityId), cancellationToken);
            if (result.IsFailure)
            {
                throw new LearnixException(nameof(GetUserPermissionsQuery), result.Error);
            }

            return result;
        }
    }
}