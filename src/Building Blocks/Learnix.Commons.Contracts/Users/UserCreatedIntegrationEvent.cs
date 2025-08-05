using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Contracts.Users
{
    public sealed record UserCreatedIntegrationEvent : IntegrationEvent
    {
        private UserCreatedIntegrationEvent()
        { }

        public UserCreatedIntegrationEvent(
            Guid correlationId,
            Guid userId,
            string firstName,
            string lastName,
            string email)
        {
            CorrelationId = correlationId;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Messagetype = GetType().Name;
        }

        public Guid UserId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
}