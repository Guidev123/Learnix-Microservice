using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record CourseCompletedDomainEvent : DomainEvent
    {
        public CourseCompletedDomainEvent(Guid studentId, Guid courseId, Guid enrollmentId, DateTime completedAt)
        {
            AggregateId = enrollmentId;
            StudentId = studentId;
            CourseId = courseId;
            EnrollmentId = enrollmentId;
            CompletedAt = completedAt;
            Messagetype = GetType().Name;
        }

        private CourseCompletedDomainEvent()
        { }

        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public Guid EnrollmentId { get; init; }
        public DateTime CompletedAt { get; init; }
    }
}