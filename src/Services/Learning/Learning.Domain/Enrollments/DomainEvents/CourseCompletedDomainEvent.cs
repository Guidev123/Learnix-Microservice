using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record CourseCompletedDomainEvent : DomainEvent
    {
        public CourseCompletedDomainEvent(Guid studentId, Guid courseId, DateTime completedAt)
        {
            AggregateId = studentId;
            StudentId = studentId;
            CourseId = courseId;
            CompletedAt = completedAt;
            Messagetype = GetType().Name;
        }

        private CourseCompletedDomainEvent()
        { }

        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public DateTime CompletedAt { get; init; }
    }
}