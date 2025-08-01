using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.DomainEvents
{
    public sealed record StudentEnrolledInCourseDomainEvent(Guid StudentId) : DomainEvent(StudentId);
}