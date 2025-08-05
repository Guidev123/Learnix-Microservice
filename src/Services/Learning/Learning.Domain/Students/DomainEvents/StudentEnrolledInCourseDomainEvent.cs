using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Students.DomainEvents
{
    public sealed record StudentEnrolledInCourseDomainEvent(
        Guid StudentId,
        Guid EnrollmentId
        ) : DomainEvent(StudentId);
}