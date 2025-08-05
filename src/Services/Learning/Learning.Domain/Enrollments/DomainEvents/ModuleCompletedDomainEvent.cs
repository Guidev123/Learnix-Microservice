using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record ModuleCompletedDomainEvent : DomainEvent
    {
        public Guid ModuleId { get; init; }
        public Guid CourseId { get; init; }
    }
}