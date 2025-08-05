using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Students.DomainEvents
{
    public sealed record StudentEnrolledInCourseDomainEvent : DomainEvent
    {
        private StudentEnrolledInCourseDomainEvent()
        { }

        public StudentEnrolledInCourseDomainEvent(Guid studentId, Guid enrollmentId)
        {
            AggregateId = studentId;
            StudentId = studentId;
            EnrollmentId = enrollmentId;
            Messagetype = GetType().Name;
        }

        public Guid StudentId { get; init; }
        public Guid EnrollmentId { get; init; }
    }
}