using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Contracts.Users
{
    public sealed record UserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}