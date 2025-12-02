using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Students.DomainEvents
{
    public sealed record StudentEnrolledInCourseDomainEvent : DomainEvent
    {
        private StudentEnrolledInCourseDomainEvent()
        { }

        public StudentEnrolledInCourseDomainEvent(Guid studentId, Guid enrollmentId, Guid courseId)
        {
            AggregateId = studentId;
            StudentId = studentId;
            EnrollmentId = enrollmentId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        public Guid StudentId { get; init; }
        public Guid EnrollmentId { get; init; }
        public Guid CourseId { get; init; }
    }
}