using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonCompletedDomainEvent : DomainEvent
    {
        public Guid LessonId { get; init; }
        public Guid ModuleId { get; init; }
    }
}