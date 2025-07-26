using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Domain.Results;

namespace Learning.Infrastructure.Authorization
{
    public sealed class PermissionService : IPermissionService
    {
        public Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}