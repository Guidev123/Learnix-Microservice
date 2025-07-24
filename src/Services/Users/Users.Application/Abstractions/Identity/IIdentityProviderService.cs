using Learnix.Commons.Domain.Results;

namespace Users.Application.Abstractions.Identity
{
    public interface IIdentityProviderService
    {
        Task<Result<string>> RegisterAsync(UserRequest request, CancellationToken cancellationToken = default);
    }
}