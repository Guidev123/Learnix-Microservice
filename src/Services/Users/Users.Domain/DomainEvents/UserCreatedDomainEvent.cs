using Learnix.Commons.Domain.DomainEvents;

namespace Users.Domain.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        Guid UserId
        ) : DomainEvent(UserId);
}