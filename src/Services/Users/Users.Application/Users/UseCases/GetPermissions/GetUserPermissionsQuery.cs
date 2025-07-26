using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Messaging;

namespace Users.Application.Users.UseCases.GetPermissions
{
    public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionResponse>;
}