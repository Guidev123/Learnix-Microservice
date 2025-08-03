using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record LessonCompletedDomainEvent(Guid LessonId, Guid ModuleId) : DomainEvent(LessonId);
}