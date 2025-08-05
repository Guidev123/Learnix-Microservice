using Learnix.Commons.Domain.DomainEvents;

namespace Users.Domain.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent : DomainEvent
    {
        private UserCreatedDomainEvent()
        { }

        public UserCreatedDomainEvent(Guid userId)
        {
            AggregateId = userId;
            UserId = userId;
            Messagetype = GetType().Name;
        }

        public Guid UserId { get; init; }
    }
}