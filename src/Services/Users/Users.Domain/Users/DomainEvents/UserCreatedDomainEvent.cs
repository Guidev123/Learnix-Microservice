using Learnix.Commons.Domain.DomainEvents;

namespace Users.Domain.Users.DomainEvents
{
    public sealed record UserCreatedDomainEvent(
        Guid UserId
        ) : DomainEvent(UserId);
}