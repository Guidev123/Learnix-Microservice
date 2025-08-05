using Learnix.Commons.Domain.DomainEvents;

namespace Users.Domain.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent : DomainEvent
    {
        public Guid UserId { get; init; }
    }
}