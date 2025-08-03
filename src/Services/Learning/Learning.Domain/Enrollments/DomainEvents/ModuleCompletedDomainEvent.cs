using Learnix.Commons.Domain.DomainEvents;

namespace Learning.Domain.Enrollments.DomainEvents
{
    public sealed record ModuleCompletedDomainEvent(Guid ModuleId, Guid CourseId) : DomainEvent(ModuleId);
}