using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonCompletedDomainEvent : DomainEvent
    {
        public LessonCompletedDomainEvent(Guid enrollmentId, Guid studentId, Guid lessonId, Guid courseId)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            LessonId = lessonId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        private LessonCompletedDomainEvent()
        { }

        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid LessonId { get; init; }
        public Guid CourseId { get; init; }
    }
}