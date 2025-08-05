using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Students.DomainEvents
{
    public sealed record StudentEnrolledInCourseDomainEvent : DomainEvent
    {
        public Guid StudentId { get; init; }
        public Guid EnrollmentId { get; init; }
    }
}