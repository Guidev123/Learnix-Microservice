using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonStartedDomainEvent : DomainEvent
    {
        public LessonStartedDomainEvent(Guid enrollmentId, Guid studentId, Guid lessonId, Guid moduleId, Guid courseId)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            LessonId = lessonId;
            ModuleId = moduleId;
            CourseId = courseId;
            Messagetype = GetType().Name;
        }

        private LessonStartedDomainEvent()
        { }

        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid LessonId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid CourseId { get; init; }
    }
}