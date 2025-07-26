using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Contracts.Users
{
    public sealed record UserCreatedIntegrationEvent(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime BirthDate
        ) : IntegrationEvent;
}