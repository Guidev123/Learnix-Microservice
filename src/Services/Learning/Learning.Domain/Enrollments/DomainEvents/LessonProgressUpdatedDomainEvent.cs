using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonProgressUpdatedDomainEvent : DomainEvent
    {
        public LessonProgressUpdatedDomainEvent(Guid enrollmentId, Guid studentId, Guid courseId, Guid moduleId, Guid lessonId, uint minutesWatched)
        {
            AggregateId = enrollmentId;
            EnrollmentId = enrollmentId;
            StudentId = studentId;
            CourseId = courseId;
            ModuleId = moduleId;
            LessonId = lessonId;
            MinutesWatched = minutesWatched;
            Messagetype = GetType().Name;
        }

        private LessonProgressUpdatedDomainEvent()
        { }

        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid LessonId { get; init; }
        public uint MinutesWatched { get; init; }
    }
}