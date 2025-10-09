using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Messaging;

namespace Users.Application.Features.GetPermissions
{
    public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionResponse>;
}