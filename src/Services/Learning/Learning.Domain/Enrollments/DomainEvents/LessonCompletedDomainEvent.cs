using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonCompletedDomainEvent : DomainEvent
    {
        private LessonCompletedDomainEvent()
        { }

        public LessonCompletedDomainEvent(Guid lessonId, Guid moduleId, Guid enrollmentId)
        {
            AggregateId = enrollmentId;
            LessonId = lessonId;
            ModuleId = moduleId;
            Messagetype = GetType().Name;
        }

        public Guid LessonId { get; init; }
        public Guid ModuleId { get; init; }
    }
}