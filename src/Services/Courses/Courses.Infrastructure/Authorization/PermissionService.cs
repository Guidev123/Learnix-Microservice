using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Infrastructure.Authorization
{
    public sealed class PermissionService : IPermissionService
    {
        public Task<Result<PermissionResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}