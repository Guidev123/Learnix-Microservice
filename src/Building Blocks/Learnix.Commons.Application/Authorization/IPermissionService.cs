using Learnix.Commons.Domain.Results;

namespace Learnix.Commons.Application.Authorization
{
    public interface IPermissionService
    {
        Task<Result<PermissionResponse>> GetUserPermissionsAsync(
            string identityId,
            CancellationToken cancellationToken = default);
    }
}